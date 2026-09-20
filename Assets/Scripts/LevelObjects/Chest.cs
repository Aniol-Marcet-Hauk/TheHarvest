using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Chest : MonoBehaviour
{
    [SerializeField] private bool m_ChestIsActive = false;
    private bool m_InRangeToOpen;
    [SerializeField] private Transform m_Pos;
    [SerializeField] private float m_JumpHeight = 1f, m_DurationJump = 1.3f;
    [SerializeField] private Animator m_An;
   
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && m_ChestIsActive == true)
        {
            GameManager.gameManager.PLAYER.setInRangeChest(true);
            m_InRangeToOpen = true;
        }
        //pos = GetComponentInChildren<Transform>().position;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && m_ChestIsActive == true)
        {
            GameManager.gameManager.PLAYER.setInRangeChest(false);
            m_InRangeToOpen = false;
        }
    }
    private void Update()
    {
        m_An.SetBool("Open", m_ChestIsActive);
        HandleOpenInput();
    }

    private void HandleOpenInput()
    {
        if (m_InRangeToOpen == true && Input.GetKeyDown(KeyCode.F))
        {
            m_InRangeToOpen = false;
            GameObject chosen;
            float rand = Random.Range(0, 100);
            switch (rand)
            {
                case < 15f:
                    chosen = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
                    break;
                case < 70f:
                    chosen = GameManager.gameManager.chooseRandItem(GameManager.gameManager.allItems);
                    break;
                case < 101f:
                    chosen = GameManager.gameManager.chooseRandUpgrade(GameManager.gameManager.allUpgrades);
                    break;
                default:
                    chosen = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
                    break;
            }

            SpawnChosen(chosen);
        }
    }
    

    private void SpawnChosen(GameObject chosen)
    {
        GameObject chosenGo = Instantiate(chosen, transform.position, chosen.transform.rotation);
        chosenGo.transform.DOJump(m_Pos.position, m_JumpHeight, 1, m_DurationJump);
        GameManager.gameManager.PLAYER.setInRangeChest(false);
        Destroy(this);
    }

    public bool chestIsActive { get => m_ChestIsActive; set => m_ChestIsActive = value; }
    public Animator an => m_An;
    



}
