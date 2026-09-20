using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Chest : MonoBehaviour
{
    public bool chestIsActive = false;
    private bool inRangeToOpen;
    [SerializeField]private Transform pos;
    [SerializeField] private float jumpHeight= 1f,durationJump = 1.3f;
    public Animator an;
   
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && chestIsActive == true)
        {
            GameManager.gameManager.PLAYER.setInRangeChest(true);
            inRangeToOpen = true;
        }
        //pos = GetComponentInChildren<Transform>().position;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && chestIsActive == true)
        {
            GameManager.gameManager.PLAYER.setInRangeChest(false);
            inRangeToOpen = false;
        }
    }
    private void Update()
    {
        an.SetBool("Open", chestIsActive);
        if(inRangeToOpen == true && Input.GetKeyDown(KeyCode.F))
        {
            
            inRangeToOpen = false;
            GameObject _chosen;
            float _rand = Random.Range(0, 100);
            switch (_rand)
            {
                case < 15f:
                    _chosen = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
                    break;
                case < 70f:
                    _chosen = GameManager.gameManager.chooseRandItem(GameManager.gameManager.allItems);
                    break;
                case < 101f:
                    _chosen = GameManager.gameManager.chooseRandUpgrade(GameManager.gameManager.allUpgrades);
                    break;
                default:
                    _chosen = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
                    break;
            }

            spawnChosen(_chosen);
        }
    }
    

    private void spawnChosen(GameObject _chosen)
    {
        GameObject _chosenGO = Instantiate(_chosen,transform.position,_chosen.transform.rotation);
        _chosenGO.transform.DOJump(pos.position, jumpHeight, 1,durationJump);
        GameManager.gameManager.PLAYER.setInRangeChest(false);
        Destroy(this);
    }
    



}
