using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Items/Healing")]
public class HealingItem : Items
{
    public string _Name = "healing";
    public float Curar = 10;
    public int _addAmount = 1;
    public Texture _imageUI;
    public float _coolDown;
    public override int addAmount()
    {
        return _addAmount;
    }
    public override string giveName()
    {
        return _Name;

    }
    public override Texture imageUI()
    {
        return _imageUI;
    }
    public override float coolDown()
    {
        return _coolDown;
    }
    public override void _Update(PlayerMain player)
    {

        GameManager.gameManager.jugadorAtacat(-Curar);
    }
}
