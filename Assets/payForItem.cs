using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class payForItem : MonoBehaviour
{
    public int price;
    public TMP_Text text;
    
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.gameManager.PLAYER.setInRangeChest(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            if(GameManager.gameManager.coins >= price)
            {
                GameManager.gameManager.coins -= price;
                bought();

            }
            else
            {
                notEnoughCoins();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.gameManager.PLAYER.setInRangeChest(false);
        }
    }
    void bought()
    {
        GameManager.gameManager.PLAYER.setInRangeChest(false);
        Destroy(gameObject);
    }
    void notEnoughCoins()
    {

    }
}
