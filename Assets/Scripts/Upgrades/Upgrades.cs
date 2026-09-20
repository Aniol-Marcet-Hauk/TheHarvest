using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[System.Serializable]
public abstract class Upgrades 
{
    //MAYBE YOU DONT NEED THE RESOURCE FOLDER,MAYBE JUST A LIST IN GAMEMANAGER WOULD BE EASIER
    public abstract string giveName();
    //public abstract Texture imageUI();
    public virtual void _Update(PlayerMain player,float strengthen)
    {

    }//INTERESTINGITEM DISTRACTION Player
    public virtual void _OnHitEnemy(PlayerMain player, Enemy en, float strengthen )
    {
        
    }
    public virtual void _OnGrab(PlayerMain player, float strengthen)
    {

    }
    public virtual void _OnUseItem(ItemsList item, float strengthen)
    {

    }
    public virtual void _OnRage(PlayerMain player, float strengthen)
    {

    }
    public virtual void _OnDie(PlayerMain player,UpgradesList upL, float strengthen)
    {

    }
    public virtual void _OnEnemyDie(PlayerMain player, float strengthen)
    {
        //Explode when die, create chain lighting when die,freeze when die ezzzz...
    }
    public virtual void _OnGetHit(PlayerMain player, float damageDealt,float strengthen)
    {

    }
}


public class FireAttacks: Upgrades
{
    public int duration = 5;
    public override string giveName()
    {
        return "FIRE";
    }
    public override void _OnHitEnemy(PlayerMain player, Enemy en, float strengthen)
    {

        en.StartCoroutine(en.constantDamage(0.25f*  strengthen, duration, 0));
        
    }
}
public class ExtraDamage : Upgrades
{

    public override string giveName()
    {
        return "Extra Damage";
    }
    public override void _OnHitEnemy(PlayerMain player, Enemy en, float strengthen)
    {

        en.MalEn(1 + strengthen,Vector3.zero);

    }
}
public class HealerUpgrade : Upgrades
{

    public override string giveName()
    {
        return "Healing Upgrade";
    }
    public override void _Update(PlayerMain player, float strengthen)
    {
        GameManager.gameManager.jugadorAtacat(- 1.1f* strengthen);
        
    }
    
}
public class MaxHealthIncrease : Upgrades
{
    public override string giveName()
    {
        return "MaxHealthIncrease";
    }
    public override void _OnGrab(PlayerMain player, float strengthen)
    {

        float vidaMax = GameManager.gameManager.jugadorVida._vidaMax + 10 * strengthen;
        GameManager.gameManager.jugadorVida.CanviarVidaMax(vidaMax);
        GameManager.gameManager.HEALTH_BAR.SetMaxHealth(vidaMax);
        float vida = GameManager.gameManager.jugadorVida._vida + 10 * strengthen;
        GameManager.gameManager.jugadorAtacat(-vida);

        
        
    }
}
//no es bo ara mateix pero potser en un futur es pot millorar?
public class ThrowItemsTriple: Upgrades
{
    public override string giveName()
    {
        return "TripleItemsThrown";
    }
    public override void _OnUseItem(ItemsList item,float strengthen)
    {
        //I DONT KNOW HOW TO
    }

}
public class RageIncrease: Upgrades
{
    private float baseValue = 15f;
    public override string giveName()
    {
        return "Rage Increase";
    }
    public override void _OnGrab(PlayerMain player, float strengthen)
    {
        player.maxRage += baseValue + 5 * strengthen;
    }
}
public class RageLastLonger : Upgrades
{
    public override string giveName()
    {
        return "Rage Last Longer";
    }
    public override void _OnGrab(PlayerMain player, float strengthen)
    {
        player.rageLowerPerSecond -= 0.25f * strengthen;
        if(player.rageLowerPerSecond < .5f)
        {
            player.rageLowerPerSecond = .5f;
        }
    }
}
public class RageReducedButStronger : Upgrades
{
    public override string giveName()
    {
        return "Rage reduced But Stronger";
    }
    public override void _OnGrab(PlayerMain player, float strengthen)
    {
        player.maxRage = 40f;
        //create a rage strength value
    }
}
public class ItemPickupDoubledEveryTime : Upgrades
{
    public override string giveName()
    {
        return "ItemPickUpDoubled";
    }
    public override void _OnGrab(PlayerMain player, float strengthen)
    {
        GameManager.gameManager.ItemPickUpAmount = 2 + ((int)strengthen);
    }
    //this would need a new void and i dont want to. option2 is giving the player a paramater that doubles pick ups or smthn
}
public class ItemStrengthen: Upgrades
{
    public override string giveName()
    {
        return "Item Stronger";
    }
    public override void _OnUseItem(ItemsList item, float strengthen)
    {
        //myb add strength paramater to itemslist???
        //I DONT REALLY LIKE THIS UPGRADE SO MAYBE ELIMINATE IT
    }
}
public class ItemCanReapear : Upgrades
{
    public override string giveName()
    {
        return "Item Can Reapear";
    }
    public override void _OnUseItem(ItemsList item, float strengthen)
    {
        float x = Random.Range(0, 100);
        if(x < 10 + 5* strengthen)
        {
            item.amount += 1;
        }

    }
}
public class IncreaseMaxItems : Upgrades
{
    public override string giveName()
    {
        return "Increse Max Items";
    }
    public override void _OnGrab(PlayerMain player, float strengthen)
    {
        player.itemsMaxAmount += ((int)strengthen);
    }
    //havent even created a max items thingy yet
}

public class TrippleShotBows : Upgrades
{
    public override string giveName()
    {
        return "TrippleShotBows";
    }
    //This would be hard...
}
public class LowerMaxHealthButActivateRageWhenHelathZero: Upgrades
{
    public override string giveName()
    {
        return "Max health -25% but Rage Activates die  ";
    }
    public override void _OnGrab(PlayerMain player, float strengthen)
    {
        GameManager.gameManager.jugadorVida.CanviarVidaMax(GameManager.gameManager.jugadorVida._vidaMax * .75f);
    }
    public override void _OnDie(PlayerMain player,UpgradesList upL, float strengthen)
    {

        GameManager.gameManager.RAGE = player.maxRage;
        player.rageModeON = true;
        player.StartCoroutine(player.RAGEMODE());


    }
    //i guess it is necessary for extra life stuff so yeah
}
public class IfHitEnemyLowerSpeed : Upgrades
{
    //NOT DONE
    public override string giveName()
    {
        return "Slow Enemy Down If Hit";
    }
    
    //I need to make all enemies have a speed variable In enemy Script
}
public class SlowTimeDownWhenRageActivated : Upgrades
{
    public override string giveName()
    {
        return "Slow Time Down When Raging";
    }
    public override void _OnRage(PlayerMain player, float strengthen)
    {
        if(.1*strengthen > 0.5)
        {
            Time.timeScale = .5f;
        }
        Time.timeScale -= .07f * strengthen;
        //add a speed multiplyer to all player animations to do this
        // A SECOND MULTIPLYER ! BUT HOW???
    }

}
public class KnockBackEnemy : Upgrades
{
    public override string giveName()
    {
        return "Knock Back Enemies";
    }
    public override void _OnHitEnemy(PlayerMain player, Enemy en, float strengthen)
    {
        if(en.GetComponent<ThrowingAndExplosive>() != null)
        {
            return;
        }
       
        en.DistanceKnockBack = 2*strengthen;

        //This second line here might not be necessary
        en.speedKnockBack = 1f * strengthen;
    }
}
public class KnockBackEnemyMeleeWeapon: Upgrades
{
    public override string giveName()
    {
        return "Knock Back When Using Melee Weapon";
    }
    //check with arma code
}
public class KnockBackEnemyRangedWeapon : Upgrades
{
    public override string giveName()
    {
        return "Knock Back When Usign Ranged Weapon";
    }
    //check with arma code
}
public class CircleOFDOOM : Upgrades
{
    public override string giveName()
    {
        return "Circle Around Player That Deals Damage To Enemies";
    }
}
public class ChainLightning : Upgrades
{
   // private int times ,maxTimes=4;
    public override string giveName()
    {
        return "Chain Lightning";
    }
    public override void _OnHitEnemy(PlayerMain player, Enemy en, float strengthen)
    {
        //LA ALTRE FORMA DE FER AIXO ÉS FICAR EN RESOURCES UN LIGHTING Q FA EL QUE FICA EN AQUEST SCRIPT
       /* if(times == maxTimes)
        {
            times = 0;
            return;
        }*/
        Collider[]  a = Physics.OverlapSphere(en.transform.position, 2 + .5f * strengthen);
        float distance =20;
        Enemy enChoose = en;

        foreach (var item in a)
        {
            Enemy _en = item.GetComponent<Enemy>();
            
            if ( _en!= null && _en != en)
            {
                float d = Vector3.Distance(en.transform.position, item.transform.position);
                if(d < distance)
                {
                    continue;
                }
                distance = d;
                enChoose = _en;
            }
        }
        if(enChoose == en)
        {
           // times = 0;
            return;
        }
        enChoose.MalEn(1f, en.transform.position);
       // times += 1;

    }
}
public class DeflectWhenRolling : Upgrades
{
    public override string giveName()
    {
        return "Deflect When Rolling";
    }
    //idk
}
public class HealPoolEverySetAmountOfTime : Upgrades
{
    public override string giveName()
    {
        return "Heal Pool Every Set Amount Of time";
    }
}
public class LavaWhenRolling: Upgrades
{
    public override string giveName()
    {
        return "Lava When Rolling";
    }
}
public class LavaWhenWalking : Upgrades
{
    public override string giveName()
    {
        return "Lava When Walking";
    }
}
public class SpeedMovement : Upgrades
{
    public override string giveName()
    {
        return "Speed Movement";
    }
    public override void _OnGrab(PlayerMain player, float strengthen)
    {
        player.velocitat += strengthen;
    }
}
public class SpeedAttacks : Upgrades
{
    public override string giveName()
    {
        return "Faster Attacks";
    }
    //u know what u gotta do
}
public class RangeAttackExplosive : Upgrades
{
    public override string giveName()
    {
        return "Range Attack Explosive";
    }
    //With this and all the other stuff, i guess i have to make a list with all the range attacks
    //OR SMART ME!!!In the onhitenemy, ADD BY which weapon it got hit(bad way to solve it but idk

}
public class MeleeAttackExplosive : Upgrades
{
    public override string giveName()
    {
        return "Melee Attack Explosive";
    }
    public override void _OnGrab(PlayerMain player, float strengthen)
    {
        GameManager.gameManager.explodeWhenMelee = true;
    }

}
public class AttackExplosive: Upgrades
{
    public override string giveName()
    {
        return "Attack Explosive";
    }
    public override void _OnHitEnemy(PlayerMain player, Enemy en, float strengthen)
    {
        //do that with the resources folder
    }
   // IEnumerator waitLongerForExplosion???

}
public class ChanceToDeflect : Upgrades
{
    public override string giveName()
    {
        return "Chance To Deflect";
    }
    public override void _OnGetHit(PlayerMain player, float damageDealt, float strengthen)
    {
        if(damageDealt > 0 && Random.Range(0f,100f) <= 10f * strengthen)
        {
            GameManager.gameManager.jugadorAtacat(-damageDealt);
        }
    }
}
public class PeriodicLazers : Upgrades
{
    public override string giveName()
    {
        return "Periodic Lazers";
    }//resources folder
}
public class ShockWaveMelee : Upgrades
{
    public override string giveName()
    {
        return "Shcok Wave Melee";
    }//resources folder
}
public class TrapDamageReduced : Upgrades
{
    public override string giveName()
    {
        return "Trap Damage Reduced";
    }//idk how to do this
}
public class Freeze : Upgrades
{
    public override string giveName()
    {
        return "Freeze";
    }
    public override void _OnHitEnemy(PlayerMain player, Enemy en, float strengthen)
    {

        en.TakeKnockBack(player.transform.position, 0, .5f + strengthen);
        en.transform.DORotateQuaternion(en.transform.rotation, .5f + strengthen);
        //ADD SOMETHING TO STUN THE ENEMY BC RIGHT NOW THEYLLE STILL ATTACK
        
    }
}
public class LavaTrailRangeWeapons : Upgrades
{
    public override string giveName()
    {
        return "Lava Trail Ranged Weapons";
    }
}
public class ConfuseEnemiesWhenHittingThem: Upgrades
{
    public override string giveName()
    {
        return "Confuse Enemies When hitting them";
    }
}
public class RevengeDamage: Upgrades
{
    public override string giveName()
    {
        return "Revenge Damage";
    }
    public override void _OnGetHit(PlayerMain player, float damageDealt, float strengthen)
    {
        //RESOURCES FOLDER EXPLOSION
        //OR JUST MAKE THE ENEMY GIVE HIS SCRIPT WHEN GIVING DAMAGE MUCH EASIER THIS WAY
    }
}
public class RevengeConfusing: Upgrades
{
    public override string giveName()
    {
        return "Revenge By Confusing";
    }
}
public class SurrouningShards : Upgrades
{
    public override string giveName()
    {
        return "Shards Surrounding The Player";
    }
}
public class RangeIncreaseFreeFlow : Upgrades
{
    public override string giveName()
    {
        return "Range Increase Ranged weapons";
    }
}
public class RangeIncreaseRangedWeapons : Upgrades
{
    public override string giveName()
    {
        return "Range Increase Ranged weapons";
    }
}
public class ResistanceToDamage : Upgrades
{
    public override string giveName()
    {
        return "Resistance To Damage";
    }
    public override void _OnGetHit(PlayerMain player, float damageDealt, float strengthen)
    {
        if(damageDealt > 0)
        {
            GameManager.gameManager.jugadorAtacat(-damageDealt * .1f * strengthen);
        }
    }
}
public class RageIncreaseDistanceMeleeAndRange: Upgrades
{
    public override string giveName()
    {
        return "Rage Increase Distance";
    }
}
public class RageRingOfLazers : Upgrades
{
    public override string giveName()
    {
        return "RingOfLazers";
    }

}
public class WhenEnterRageExplosion : Upgrades
{
    public override string giveName()
    {
        return "When Enter Rage Explosion";
    }
    public override void _OnRage(PlayerMain player, float strengthen)
    {
        
    }
}
public class BackStabDamageIncrease : Upgrades
{
    public override string giveName()
    {
        return "Back Stab Damage Increase";
    }
    public override void _OnHitEnemy(PlayerMain player, Enemy en, float strengthen)
    {
        Vector3 pos = (en.transform.position - player.transform.position).normalized;
        float dotProduct = Vector3.Dot(en.transform.forward, pos);
        if(dotProduct < -0.5)
        {
            en.MalEn(2.5f + strengthen,Vector3.zero);
        }
        
        
    }
}
public class RandomItemEveryXAmountOfTime : Upgrades
{
    //CHANGE WHEN THERE IS A MAX ITEM
    //ADD UPGRADE LIST TO _UPDATE!!!!!IMPORTANT!!!!
    //THIS IS MEGA LEGENDARY
    float time = 0;
    float maxTime = 15f;
    int amountOfItems = 10;
    int timesDone;
    public override string giveName()
    {
        return "Random Item Every X Amount Of Time";
    }
    public override void _Update(PlayerMain player, float strengthen)
    {
        if (timesDone == amountOfItems)
        {
            return;
        }
        time += strengthen;
        if(time >= maxTime)
        {
            
            if (player.itemsMaxAmount == player.itemsL.Count)
            {
                Debug.Log("WaitingForSpaceToBeAvailable");
                return;


            }
            timesDone += 1;
            time = 0;
            ItemHolder item = GameManager.gameManager.chooseRandItem(GameManager.gameManager.allItems).GetComponent<ItemHolder>();
            
            item.addItem(player);
            
            
        }
    }
}
public class PETSMAYBE : Upgrades
{
    public override string giveName()
    {
        return "PetsMaybe";
    }
}
public class MAPFULLMAYBE : Upgrades
{
    public override string giveName()
    {
        return "Map fully revealed FromThe Beggining";
    }
}
public class SecondLife : Upgrades
{
    public override string giveName()
    {
        return "Second Life";
    }
    public override void _OnDie(PlayerMain player, UpgradesList upL,float strengthen)
    {
        if(strengthen > 0)
        {
            GameManager.gameManager.jugadorVida.CanviarVida(GameManager.gameManager.jugadorVida._vidaMax*.5f);
            upL.strength -= 1;
            if(upL.strength == 0)
            {
                GameManager.gameManager.upgradesList.Remove(upL);
            }
        }
        
    }
}
public class DamageIncreaseWhenRage: Upgrades
{
    public override string giveName()
    {
        return "Damage Increase When Rage";
    }
    public override void _OnHitEnemy(PlayerMain player, Enemy en, float strengthen)
    {
        if(player.rageModeON == true)
        {
            en.MalEn(1.5f + strengthen, Vector3.zero);
        }



    }

}



