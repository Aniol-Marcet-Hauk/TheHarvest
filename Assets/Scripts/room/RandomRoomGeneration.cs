using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class RandomRoomGeneration : MonoBehaviour
{
    public GameObject _playerGO;
    public List<Room> allRooms;
    public List<Room> roomsLevel;
    public Room EXITROOMPREFAB;
    public List<Room> roomsThatAllLevelsShouldHaveEdge;
    public List<Room> edgeRooms;
    public Room start, exit;
    public int numberOfRooms = 10;//doesn't includeStartingRoom
    private bool resetLevelGen, levelFinished = false;
    [SerializeField] private GameObject empty;
    public List<GameObject> ENEMIES;
    //[SerializeField] private GameObject LoadingScene;
    [SerializeField] private int numberOfRoomsSet;
    private Transform player;
    private List<int> probability = new List<int>();
    private bool playerInstantiate = true;
    /*public float mean, stdDev;
    public GameObject plot;*/
    private void Start()
    {
        for(int r = 0; r<allRooms.Count; r++)
        {
            for (int i = 0; i < allRooms[r].probability; i++)
            {
                probability.Add(r);
            }
        }
        
        
       




        StartCoroutine(GenerateLevel());
        StartCoroutine(ResetLevelGen());

    }
    private IEnumerator GenerateLevel()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(.1f);
        GameManager.gameManager.PLAYER = FindObjectOfType<PlayerMain>();
        if (GameManager.gameManager.PLAYER != null)
        {
            playerInstantiate = false;

            GameManager.gameManager.PLAYER.gameObject.SetActive(false);
        }
        int randomIndex = Random.Range(0, allRooms.Count);

        //primera habitaciò

        roomsLevel.Add(Instantiate(allRooms[0].gameObject).GetComponent<Room>());

        //maybe?
        for (int i = 0; i < allRooms[0].probability; i++)
        {
            probability.Remove(0);
        }
 
       
        
        while(roomsLevel.Count <= numberOfRooms)
        {
            yield return new WaitForFixedUpdate();
            //si no ha pogut crear el nivell torna a començar
            if (resetLevelGen == true)
            {
                break;
            }
            //tria una habitaciò random i una porta de aquella habitaciò random
            randomIndex = ChooseRandomRoom();

            int door1 = ChooseDoor(allRooms[randomIndex].doors);
            //tria porta en que es conectarà
            int randomRoomForConnection = Random.Range(0, roomsLevel.Count);
            int doorConnected= ChooseDoor(roomsLevel[randomRoomForConnection].doors);
            if(doorConnected == -1)
            {
                
                continue;
            }
            //gira l'habitaciò i posa-la a la posciò correcta
            Transform door1T = allRooms[randomIndex].doors[door1].transform;
            Transform doorConnectedT = roomsLevel[randomRoomForConnection].doors[doorConnected].transform;
            
            Quaternion rotate = Quaternion.Euler(0f, doorConnectedT.eulerAngles.y - door1T.eulerAngles.y + 180, 0);
     
            Room room = Instantiate(allRooms[randomIndex].gameObject,Vector3.one *50,rotate).GetComponent<Room>();
            Vector3 position = (doorConnectedT.position) +( room.transform.position- room.doors[door1].transform.position);
            position.y = 0;
            room.transform.position = position;
            yield return new WaitForSeconds(0.1f);
            if(room.isTouching == true)
            {
                Destroy(room.gameObject);
                continue;
            }
            //Afageix habitaciò a llista d'habitacions 
            room.doors[door1].connectedRoom = roomsLevel[randomRoomForConnection];
            
            
            roomsLevel.Add(room);
            roomsLevel[randomRoomForConnection].doors[doorConnected].connectedRoom = room;

            if(room.onlyOnce == true)
            {
                for (int i = 0; i < allRooms[randomIndex].probability; i++)
                {
                    probability.Remove(randomIndex);
                }
               // allRooms.RemoveAt(randomIndex);

            }
            //allRooms.RemoveAt(randomIndex);
        }
        if(resetLevelGen == true)
        {
            resetLEVEL();
            yield break;
        }
        //set Rooms
        start = roomsLevel[0];
        exit = roomsLevel[0];
        SetRoomsGenAfterLevelGen(start);
        yield return new WaitUntil(() =>numberOfRooms == (numberOfRoomsSet-1));
        if(edgeRooms.Count < 3)
        {
            resetLEVEL();
            yield break;
        }
        foreach (Room r in edgeRooms)
        {
            if(r.distanceFromStart > exit.distanceFromStart)
            {
                exit = r;
            }
        }
        int amount = roomsThatAllLevelsShouldHaveEdge.Count;
        int edgeSpawned = 0;
        foreach (Room r in edgeRooms)
        {
            int rand = Random.Range(0, amount);
            if (r == exit)
            {
                Room conRoom = new Room();
                door conDoor = new door();
                foreach (door d in r.doors)
                {
                    if (d.connectedRoom != null)
                    {
                        conRoom = d.connectedRoom;

                        foreach (door d2 in conRoom.doors)
                        {
                            if (d2.connectedRoom == r)
                            {
                                conDoor = d2;
                            }
                        }
                    }
                }
                roomsLevel.Remove(r);
                Destroy(r.gameObject);
                Transform door1T = EXITROOMPREFAB.doors[0].transform;
                Transform doorConnectedT = conDoor.transform;

                Quaternion rotate = Quaternion.Euler(0f, doorConnectedT.eulerAngles.y - door1T.eulerAngles.y + 180, 0);

                Room room = Instantiate(EXITROOMPREFAB, Vector3.one * 50, rotate).GetComponent<Room>();//check if you really need .getcomponent or not
                Vector3 position = (doorConnectedT.position) + (room.transform.position - room.doors[0].transform.position);
                position.y = 0;
                room.transform.position = position;
                room.doors[0].connectedRoom = conRoom;


                roomsLevel.Add(room);
                conDoor.connectedRoom = room;
                room.doors[0].minimapDoor.SetActive(true);
                exit = room;
            }
            else if(edgeSpawned < 2)
            {
                Room conRoom = new Room();
                door conDoor = new door();
                foreach(door d in r.doors)
                {
                    if(d.connectedRoom != null)
                    {
                        conRoom = d.connectedRoom;
                        foreach(door d2 in conRoom.doors)
                        {
                            if(d2.connectedRoom == r)
                            {
                                conDoor = d2;
                            }
                        }
                    }
                }
                roomsLevel.Remove(r);
                Destroy(r.gameObject);
                Transform door1T = roomsThatAllLevelsShouldHaveEdge[rand].doors[0].transform;
                Transform doorConnectedT = conDoor.transform;

                Quaternion rotate = Quaternion.Euler(0f, doorConnectedT.eulerAngles.y - door1T.eulerAngles.y + 180, 0);

                Room room = Instantiate(roomsThatAllLevelsShouldHaveEdge[rand].gameObject, Vector3.one * 50, rotate).GetComponent<Room>();//check if you really need .getcomponent or not
                Vector3 position = (doorConnectedT.position) + (room.transform.position - room.doors[0].transform.position);
                position.y = 0;
                room.transform.position = position;
                room.doors[0].connectedRoom = conRoom;


                roomsLevel.Add(room);
                conDoor.connectedRoom = room;
                room.doors[0].minimapDoor.SetActive(true);
                roomsThatAllLevelsShouldHaveEdge.RemoveAt(rand);
                edgeSpawned += 1;
                amount -= 1;
            }
            
        }
        //UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
        //yield return new WaitForSeconds(3f);
        Vector3 plPosStart = start.transform.position;
        if(playerInstantiate == false)
        {
            GameManager.gameManager.PLAYER.transform.position = new Vector3(start.transform.position.x,1.05f, start.transform.position.z);
            GameManager.gameManager.PLAYER.transform.rotation = _playerGO.transform.rotation;
            GameManager.gameManager.PLAYER.gameObject.SetActive(true);
            player = GameManager.gameManager.PLAYER.transform;
            yield return new WaitForSeconds(.2f);
            GameManager.gameManager.loadingScene.SetActive(false);
           
        }
        else
        {
            
            player = Instantiate(_playerGO, start.transform.position, _playerGO.transform.rotation).transform;
            DontDestroyOnLoad(player);
            GameManager.gameManager.PLAYER = player.GetComponentInChildren<PlayerMain>();
            player = GameManager.gameManager.PLAYER.transform;
            GameManager.gameManager.loadingScene.SetActive(false);
        }
        
        levelFinished = true;
        
        
        
        
    }
    private void SetRoomsGenAfterLevelGen(Room currentRoom)
    {

        int x = 0;
        foreach (door item in currentRoom.doors)
        {
            Room nextRooms = item.connectedRoom;
            if (nextRooms == null)
            {
                if(item.doorPrefab != null)
                {
                    item.doorPrefab.SetActive(false);
                }
                
                //spawn wall
                continue;
            }
            x += 1;
            //spawn door
            item.minimapDoor.SetActive(true);
            item.doorObj.gameObject.SetActive(false);
            if (nextRooms.distanceFromStart == 0 && nextRooms != start || nextRooms.distanceFromStart > currentRoom.distanceFromStart +1)
            {
                nextRooms.distanceFromStart = currentRoom.distanceFromStart + 1;
                SetRoomsGenAfterLevelGen(nextRooms);
            }

      

        }
        if(currentRoom.hasSet == true)
        {
            return;
        }
        numberOfRoomsSet += 1;
        currentRoom.hasSet = true;

        if (x != 1)
        {
            if (currentRoom.cases != 0)
            {
                /*int max = currentRoom.cases;
                if (currentRoom.cases > currentRoom.distanceFromStart) { max = currentRoom.distanceFromStart; } 
                 */


                //in the case of wanting to have different generation, i will have to have a list with the last enemies
                //currentRoom.InstantiateEnemies(Random.Range(0, currentRoom.cases));
            }
           
            return;
        }
        if(currentRoom != start)
        {
            edgeRooms.Add(currentRoom);
        }
        
        

        //aixo és si necessito començar triobant tots els edge rooms
        /*
        for (int i = 0; i < roomsLevel.Count; i++)
        {
            int x = 0;
            foreach (door item in roomsLevel[i].doors)
            {
                if (item.connectedRoom != null)
                {
                    x += 1;
                    //spawn door
                    continue;
                }
                //spawn wall


            }
            if (x != 1)
            {
                if (roomsLevel[i].cases != 0)
                {
                    roomsLevel[i].InstantiateEnemies(Random.Range(0, roomsLevel[i].cases));
                }

                continue;
            }
            edgeRooms.Add(roomsLevel[i]);
            if (start == null || start.transform.position.magnitude < roomsLevel[i].transform.position.magnitude)
            {
                start = roomsLevel[i];
            }

        }*/

        //this one will probably be done differently
        //una altre forma seria anar desde la start room, 1 per 1 ficant el numero de enemics... i després mirar quina està més lluny per ficar el exit
        //aquest sistema que dic a dalt seria millor, a més a més, així també puc ficar tots els enemics al mateix temps.
        /*float distExit = 0f;
        foreach (Room item in edgeRooms)
        {
            if (item == start)
            {
                continue;

            }
            float dist = Vector3.Distance(start.transform.position, item.transform.position);
            if (dist > distExit)
            {
                distExit = dist;
                exit = item;
            }
        }*/
    }
    void resetLEVEL()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        /*resetLevelGen = false;
        for (int i = 0; i < roomsLevel.Count; i++)
        {
            Destroy(roomsLevel[i].gameObject);
        }
        StartCoroutine(GenerateLevel());
        StartCoroutine(ResetLevelGen());*/
    }
    IEnumerator ResetLevelGen()
    {
        yield return new WaitForSeconds(6f);
        if(levelFinished == false || roomsLevel.Count < numberOfRooms)
        {
            resetLEVEL();
        }
    }
    private int ChooseDoor(List<door> _doors)
    {
        for (int i = 0; i < 6; i++)
        {
            int randomDoor = Random.Range(0, _doors.Count);

            if (_doors[randomDoor].connectedRoom == null)
            {

                return randomDoor;
            }
        }
        return -1;
        
        
    }

    public GameObject placeEnemies(Enemies _En)
    {
        
        

        switch (_En)
        {
            
            case Enemies.Wolf:
                return ENEMIES[0];
            case Enemies.FaceScream:
                return ENEMIES[1];
            case Enemies.RandomMove:
                return ENEMIES[2];
            case Enemies.SimpleShooter:
                return ENEMIES[3];
            case Enemies.BarrelThrower:
                return ENEMIES[4];
            case Enemies.RunsAway:
                return ENEMIES[5];
            case Enemies.BasicPuncher:
                return ENEMIES[6];
            default:
                return empty;

        }
    }

    int ChooseRandomRoom()
    {
        int rand = Random.Range(0, probability.Count);
        return probability[rand];

        /*float u1 = 1f - Random.Range(0f, 1f);
        float u2 = 1f - Random.Range(0f, 1f);
        float  randStdNormal = Mathf.Sqrt(-2f* Mathf.Log(u1)) *
                     Mathf.Sin(2f * Mathf.PI * u2); //random normal(0,1)
        float randNormal = mean + stdDev * randStdNormal;
        
        return randNormal*randNormal;*/
    }
    private void Update()
    {
        if(player == null)
        {

            return;
        }
        foreach(Room r in roomsLevel)
        {
            if(Vector3.Distance(player.position,r.transform.position) <= 25*r.roomSize )
            {
                if (r.graphics.activeInHierarchy)
                {
                    continue;
                }
                r.graphics.SetActive(true);
            }
            else if(r.graphics.activeInHierarchy)
            {
                r.graphics.SetActive(false);
            }
        }
    }

}

