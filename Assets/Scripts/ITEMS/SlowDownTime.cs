using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/SlowDownTime")]
public class SlowDownTime : Items
{
    public string _Name = "slowDownTime";
    public Texture _imageUI;

    public int _addAmount = 1;
    public float timeSlowDown;
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
        Time.timeScale = timeSlowDown;
    }
}
