using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/Armadura")]
public class Armour : Items
{
    public string _Name = "Armadura";
    public float reduccioDeMalMenorA1, temps;
    public bool invincibilitat = false;
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
        if(invincibilitat == true)
        {
            GameManager.gameManager.PLAYER.invincibilityAfterHit(temps);
            return;
        }
        float x = GameManager.gameManager.playerDamageMultiplier;
        GameManager.gameManager.playerDamageMultiplier = reduccioDeMalMenorA1;
        playerDamageBackToNormal(x);
    }
    IEnumerator playerDamageBackToNormal(float back)
    {
        yield return new WaitForSeconds(temps);
        GameManager.gameManager.playerDamageMultiplier = back;
    }
}

