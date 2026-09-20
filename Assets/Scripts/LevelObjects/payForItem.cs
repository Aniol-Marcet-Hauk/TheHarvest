using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class payForItem : MonoBehaviour
{
    [SerializeField] private int m_Price;
    [SerializeField] private TMP_Text m_Text;
    
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
            if(GameManager.gameManager.coins >= m_Price)
            {
                GameManager.gameManager.coins -= m_Price;
                Bought();

            }
            else
            {
                NotEnoughCoins();
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
    private void Bought()
    {
        GameManager.gameManager.PLAYER.setInRangeChest(false);
        Destroy(gameObject);
    }
    private void NotEnoughCoins()
    {

    }

    public int price { get => m_Price; set => m_Price = value; }
    public TMP_Text text => m_Text;
}
