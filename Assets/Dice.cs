using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Dice : MonoBehaviour
{
    public RawImage[] images;
    public Texture2D bad, good;
    private bool inRangeToOpen,opened;
    public float  timerotating;
    public Room thisRoom;
    public Collider coll1, coll2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.gameManager.PLAYER.setInRangeChest(true);
            inRangeToOpen = true;
        }
      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.gameManager.PLAYER.setInRangeChest(false);
            inRangeToOpen = false;
        }
    }
   
    void Update()
    {
        if (inRangeToOpen == true && Input.GetKeyDown(KeyCode.F) && opened ==false)
        {
            opened = true;

            StartCoroutine(Choose());
        }
    }
    IEnumerator Choose()
    {
        GameManager.gameManager.PLAYER.setInRangeChest(false);
        coll1.enabled = false;
        coll2.enabled = false;
        transform.DOShakeRotation(timerotating,90,10,360);
        yield return new WaitForSeconds(timerotating-.4f);
        int gvsb = Random.Range(0, 2);
        transform.DOPunchScale(Vector3.one * 1.3f, .25f);
        if(gvsb == 1)
        {
            foreach (var item in images)
            {
                item.texture = good;
            }
            yield return new WaitForSeconds(.6f);
            
            Good();
        }
        else
        {
            foreach (var item in images)
            {
                item.texture = bad;
            }
            yield return new WaitForSeconds(.6f);
            
            Bad();
        }
        transform.GetComponent<MeshRenderer>().enabled = false;
       
        foreach (var item in images)
        {
            item.gameObject.SetActive(false);
        }
       


    }
    
    void Bad()
    {

        thisRoom.SetPlayerHasPassed(true);
        thisRoom.RoomEntered();
    }
    private void Good()
    {
        GameObject _chosen;
        float _rand = Random.Range(0, 100);
        switch (_rand)
        {
            case < 10f:
                _chosen = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
                spawnChosen(_chosen);
                break;
            case < 70f:
                int x = Random.Range(1, 4);
                for (int i = 0; i < x; i++)
                {
                    _chosen = GameManager.gameManager.chooseRandItem(GameManager.gameManager.allItems);
                    spawnChosen(_chosen);
                }
                break;
            case < 100f:
                int x2 = Random.Range(1, 3);
                for (int i = 0; i < x2; i++)
                {
                    _chosen = GameManager.gameManager.chooseRandUpgrade(GameManager.gameManager.allUpgrades);
                    spawnChosen(_chosen);
                }
                break;
            default:
                _chosen = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
                spawnChosen(_chosen);
                break;
        }
    }
    private void spawnChosen(GameObject _chosen)
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
}
