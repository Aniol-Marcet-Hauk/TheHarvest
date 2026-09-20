using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public Items Item;
    public int price;
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
            
            addItem(pm);
            
        }
    }
   
    public void addItem(PlayerMain _pm)
    {
        if(_pm.itemsL== null || _pm.itemsL.Count == 0)
        {
            _pm.itemsL.Add(new ItemsList(Item, Item.giveName(), Item.addAmount() * GameManager.gameManager.ItemPickUpAmount, Item.coolDown()));
            Destroy(gameObject);
            return;
        }
        foreach(ItemsList i in _pm.itemsL)
        {
            if(i.itemName == Item.giveName())
            {
                i.amount += Item.addAmount() * GameManager.gameManager.ItemPickUpAmount ;
                Destroy(gameObject);
                return;
            }
        }
        if(_pm.itemsL.Count == _pm.itemsMaxAmount)
        {
            return;
        }
        _pm.itemsL.Add(new ItemsList(Item, Item.giveName(), Item.addAmount() * GameManager.gameManager.ItemPickUpAmount, Item.coolDown()));
        Destroy(gameObject);
    }
   
}
