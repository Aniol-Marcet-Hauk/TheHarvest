using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private Items m_Item;
    [SerializeField] private int m_Price;
    //public AllItems chooseItem;
    
    /*private void Start()
    {
        Item = Assign(chooseItem);
    }*/
    private void OnTriggerEnter(Collider other)
    {
        PlayerMain pm = other.GetComponent<PlayerMain>();
        if (pm != null||other.tag == "Player")
        {
            
            AddItem(pm);
            
        }
    }
   
    public void AddItem(PlayerMain _pm)
    {
        if(_pm.itemsL== null || _pm.itemsL.Count == 0)
        {
            _pm.itemsL.Add(new ItemsList(m_Item, m_Item.giveName(), m_Item.addAmount() * GameManager.gameManager.ItemPickUpAmount, m_Item.coolDown()));
            Destroy(gameObject);
            return;
        }
        foreach(ItemsList i in _pm.itemsL)
        {
            if(i.itemName == m_Item.giveName())
            {
                i.amount += m_Item.addAmount() * GameManager.gameManager.ItemPickUpAmount ;
                Destroy(gameObject);
                return;
            }
        }
        if(_pm.itemsL.Count == _pm.itemsMaxAmount)
        {
            return;
        }
        _pm.itemsL.Add(new ItemsList(m_Item, m_Item.giveName(), m_Item.addAmount() * GameManager.gameManager.ItemPickUpAmount, m_Item.coolDown()));
        Destroy(gameObject);
    }

    public void addItem(PlayerMain pm) => AddItem(pm);
    public Items Item => m_Item;
    public int price => m_Price;
   
}
