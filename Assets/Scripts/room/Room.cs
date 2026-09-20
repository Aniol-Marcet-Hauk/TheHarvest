using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;



    

public class Room : MonoBehaviour
{
    public GameObject roomPrefab;
 
    public List<door> doors;
    public bool isTouching = false;
    public int cases;
    public List<EnemySpawn> Spawners;
    public List<Chest> Chests;
    private RandomRoomGeneration randRoomGen;
    public int distanceFromStart = 0;
    public bool hasSet;
    private bool m_PlayerHasPassed;
    public List<GameObject> enemiesSpawned;
    public bool CompletedRoom;
    public bool onlyOnce = false;
    //this will be in GameManager

    //
    public NavMeshData m_NavMeshData;
    private NavMeshDataInstance m_NavMeshInstance;

    [field:SerializeReference]public float roomSize { get; private set; }
    public GameObject graphics;
    [Space]
    [SerializeField] private bool m_SpawnEnemies = true;
    [Space]
    public int probability = 1;
    private bool m_SpawnedEnemies;
    void OnDisable()
    {
        NavMesh.RemoveNavMeshData(m_NavMeshInstance);
    }
    private void Start()
    {
        if(Spawners.Count != 0)
        {
            cases = Spawners[0].cases.Count;
        }
        randRoomGen = FindObjectOfType<RandomRoomGeneration>();
        for (int i = 0; i <doors.Count; i++)
        {
            doors[i].SetThisRoom(this);
        }
       
        

    }
    private void Update()
    {
        HandleRoomCompletionInUpdate();
    }

    private void HandleRoomCompletionInUpdate()
    {
        if (m_PlayerHasPassed == true && CompletedRoom == false && m_SpawnedEnemies)
        {
            if (enemiesSpawned.Count != 0)
            {
                enemiesSpawned.RemoveAll(GameObject => GameObject == null);
            }
            else
            {
                CompletedRoom = true;

                NavMesh.RemoveNavMeshData(m_NavMeshInstance);

                foreach (door d in doors)
                {
                    if (d.connectedRoom != null)
                    {
                        d.doorObj.SetActive(false);
                    }
                }
                if (Chests.Count == 0)
                {
                    return;
                }
                foreach (Chest i in Chests)
                {
                    i.chestIsActive = true;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RoomCollider")
        {
            isTouching = true;
        }
        else if( m_PlayerHasPassed == false && other.GetComponent<PlayerMain>() != null && m_SpawnEnemies == true)
        {
            m_PlayerHasPassed = true;
            
            RoomEntered();
            
        }
    }
   
    
    public void RoomEntered()
    {
        foreach(door d in doors)
        {
            d.doorObj.SetActive(true);
        }
       
        StartCoroutine(InstantiateEnemies(Random.Range(0, cases)));
        
        

    }
    public IEnumerator InstantiateEnemies(int chooseCase)
    {

        if (m_NavMeshData != null)
        {
            m_NavMeshData.position = transform.position;
            m_NavMeshData.rotation = transform.rotation;
            m_NavMeshInstance = NavMesh.AddNavMeshData(m_NavMeshData);
        }
        yield return new WaitForSeconds(.3f);
        m_SpawnedEnemies = true;
        if (cases != 0)
        {
            for (int i = 0; i < Spawners.Count; i++)
            {
                GameObject enemGO = randRoomGen.placeEnemies(Spawners[i].randomEN(Spawners[i].cases[chooseCase]));
                if (enemGO == null)
                {
                    //Debug.Log("NULLLLLL!!!");
                    continue;
                }
                enemiesSpawned.Add(Instantiate(enemGO, Spawners[i].transform));

            }
        }
            
        
    }
    public void SetPlayerHasPassed(bool p)
    {
        m_PlayerHasPassed = p;
    }
}
