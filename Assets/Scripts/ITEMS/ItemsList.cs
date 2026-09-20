using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemsList 
{
    public Items item;
    public string itemName;
    public int amount;
    public float coolDown;
    public ItemsList(Items _Item, string _name, int _stack,float _coolDown)
    {
        item = _Item;
        itemName = _name;
        amount = _stack;
        coolDown = _coolDown;
    }
}

[System.Serializable]
public class UpgradesList
{
    public Upgrades upgrade;
    public string itemName;
    public float strength;
    //public int duration = 0;
    public UpgradesList(Upgrades _upgrade, string _name, float _strength)
    {
        upgrade = _upgrade;
        itemName = _name;
        strength = _strength;
        
    }
}
