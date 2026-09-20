using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Dice : MonoBehaviour
{
    [SerializeField] private RawImage[] m_Images;
    [SerializeField] private Texture2D m_Bad, m_Good;
    private bool m_InRangeToOpen, m_Opened;
    [SerializeField] private float m_TimeRotating;
    [SerializeField] private Room m_ThisRoom;
    [SerializeField] private Collider m_Coll1, m_Coll2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.gameManager.PLAYER.setInRangeChest(true);
            m_InRangeToOpen = true;
        }
      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.gameManager.PLAYER.setInRangeChest(false);
            m_InRangeToOpen = false;
        }
    }
   
    void Update()
    {
        HandleOpenInput();
    }

    private void HandleOpenInput()
    {
        if (m_InRangeToOpen == true && Input.GetKeyDown(KeyCode.F) && m_Opened == false)
        {
            m_Opened = true;

            StartCoroutine(Choose());
        }
    }
    IEnumerator Choose()
    {
        GameManager.gameManager.PLAYER.setInRangeChest(false);
        m_Coll1.enabled = false;
        m_Coll2.enabled = false;
        transform.DOShakeRotation(m_TimeRotating,90,10,360);
        yield return new WaitForSeconds(m_TimeRotating-.4f);
        int gvsb = Random.Range(0, 2);
        transform.DOPunchScale(Vector3.one * 1.3f, .25f);
        if(gvsb == 1)
        {
            foreach (var item in m_Images)
            {
                item.texture = m_Good;
            }
            yield return new WaitForSeconds(.6f);
            
            Good();
        }
        else
        {
            foreach (var item in m_Images)
            {
                item.texture = m_Bad;
            }
            yield return new WaitForSeconds(.6f);
            
            Bad();
        }
        transform.GetComponent<MeshRenderer>().enabled = false;
       
        foreach (var item in m_Images)
        {
            item.gameObject.SetActive(false);
        }
       


    }
    
    void Bad()
    {

        m_ThisRoom.SetPlayerHasPassed(true);
        m_ThisRoom.RoomEntered();
    }
    private void Good()
    {
        GameObject _chosen;
        float _rand = Random.Range(0, 100);
        switch (_rand)
        {
            case < 10f:
                _chosen = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
                SpawnChosen(_chosen);
                break;
            case < 70f:
                int x = Random.Range(1, 4);
                for (int i = 0; i < x; i++)
                {
                    _chosen = GameManager.gameManager.chooseRandItem(GameManager.gameManager.allItems);
                    SpawnChosen(_chosen);
                }
                break;
            case < 100f:
                int x2 = Random.Range(1, 3);
                for (int i = 0; i < x2; i++)
                {
                    _chosen = GameManager.gameManager.chooseRandUpgrade(GameManager.gameManager.allUpgrades);
                    SpawnChosen(_chosen);
                }
                break;
            default:
                _chosen = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
                SpawnChosen(_chosen);
                break;
        }
    }
    private void SpawnChosen(GameObject _chosen)
    {
        GameObject _chosenGO = Instantiate(_chosen, transform.position, _chosen.transform.rotation);
       
        Vector3 vecn = Vector3.zero;
        do
        {
            vecn = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized * 3f;
        }
        while (vecn == Vector3.zero);

        _chosenGO.transform.DOJump(transform.position + vecn, 1.5f, 1, 1);


    }

    public Room thisRoom { get => m_ThisRoom; set => m_ThisRoom = value; }
}
