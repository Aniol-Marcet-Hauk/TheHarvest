using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class Items: ScriptableObject
{

    public abstract string giveName();
    public abstract int addAmount();
    public abstract float coolDown();
    public abstract Texture imageUI();
    
    public virtual void _Update(PlayerMain player)
    {

    }
    

}
/*[CreateAssetMenu(menuName = "Healing")]
public class Healing : Items
{
    public string _Name = "healing";
    public float Curar = 10;
    public int _addAmount = 1;

    public override int addAmount()
    {
        return _addAmount;
    }
    public override string giveName()
    {
        return _Name;
       
    }
    
    public override void _Update(PlayerMain player)
    {
        Debug.Log("curat");
        GameManager.gameManager.jugadorAtacat(-Curar);
    }

    
}*/
/*[CreateAssetMenu(menuName = "Knives")]
public class knife : Items
{
    public string _Name = "Knife";
    public float Damage = 2;
    public int _addAmount = 3;

    public override int addAmount()
    {
        return _addAmount;
    }
    public override string giveName()
    {
        return _Name;
    }
    
    public override void _Update(PlayerMain player)
    {
        //do something
    }
}*/
