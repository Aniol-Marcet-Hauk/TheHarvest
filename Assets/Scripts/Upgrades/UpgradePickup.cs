using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    private Upgrades upgr;
    public float strength = 1;
    public ChooseUpgrade chooseUpgrade;
    public int price;
    private void Start()
    {
        upgr = assaignUpgr();
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerMain pm = other.GetComponent<PlayerMain>();
        if (pm != null)
        {

            AddUpgradeToUpgradeList();
            GameManager.gameManager.StartCoroutine(GameManager.gameManager.UpgradeShow(chooseUpgrade.ToString()));
            Destroy(gameObject);
        }
    }
    
    void AddUpgradeToUpgradeList()
    {
        List<UpgradesList> upgrL = GameManager.gameManager.upgradesList;
        foreach(UpgradesList i in GameManager.gameManager.upgradesList)
        {
            if(i.itemName == upgr.giveName())
            {
                i.strength += strength;
                RunUpgradeOnGrab(i,strength);
                return;
            }

        }
        UpgradesList _up = new UpgradesList(upgr, upgr.giveName(), strength);
        GameManager.gameManager.upgradesList.Add(_up);
        RunUpgradeOnGrab(_up,_up.strength);
    }
    void RunUpgradeOnGrab(UpgradesList u,float _strength)
    {

        u.upgrade._OnGrab(GameManager.gameManager.PLAYER, _strength);

    }
  
    public Upgrades assaignUpgr()
    {
        switch (chooseUpgrade)
        {
            case ChooseUpgrade.Healing:
                return new HealerUpgrade();
            case ChooseUpgrade.Fire:
                return new FireAttacks();
            case ChooseUpgrade.ExtraDamage:
                return new ExtraDamage();
            case ChooseUpgrade.MaxHealthIncrease:
                return new MaxHealthIncrease();
            case ChooseUpgrade.RageIncrease:
                return new RageIncrease();
            case ChooseUpgrade.RageLastLonger:
                return new RageLastLonger();
            case ChooseUpgrade.ItemPickupAmountDoubled:
                return new ItemPickupDoubledEveryTime();
            case ChooseUpgrade.ItemCanReapear:
                return new ItemCanReapear();
            case ChooseUpgrade.ItemMaxIncrease:
                return new IncreaseMaxItems();
            case ChooseUpgrade.LowerHealthButActivateRageWhenDeath:
                return new LowerMaxHealthButActivateRageWhenHelathZero();
            case ChooseUpgrade.KnockBackUpgrade:
                return new KnockBackEnemy();
            case ChooseUpgrade.ChainLightning:
                return new ChainLightning();
            case ChooseUpgrade.SpeedUpMovement:
                return new SpeedMovement();
            case ChooseUpgrade.MeleeAttackExplosive:
                return new MeleeAttackExplosive();
            case ChooseUpgrade.ChanceToDeflect:
                return new ChanceToDeflect();
            case ChooseUpgrade.Freeze:
                return new Freeze();
            case ChooseUpgrade.BackStabDamageIncrease:
                return new BackStabDamageIncrease();
            case ChooseUpgrade.Random10ItemsEveryXAmountOfTime:
                return new RandomItemEveryXAmountOfTime();
            case ChooseUpgrade.SecondLife:
                return new SecondLife();
            case ChooseUpgrade.damageIncreaseWhenRage:
                return new DamageIncreaseWhenRage();
            default:
                return new HealerUpgrade();

        }
    }
   
}
public enum ChooseUpgrade
{
    Healing,
    Fire,
    ExtraDamage,
    MaxHealthIncrease,
    RageIncrease,
    RageLastLonger,
    ItemPickupAmountDoubled,
    ItemCanReapear,
    ItemMaxIncrease,
    LowerHealthButActivateRageWhenDeath,
    KnockBackUpgrade,
    ChainLightning,//not FINISHED
    SpeedUpMovement,
    MeleeAttackExplosive,
    ChanceToDeflect,
    Freeze, //not FINISHED
    ResistanceToDamage,//SHOULD CHANGE HOW YOU HEAL
    BackStabDamageIncrease,
    Random10ItemsEveryXAmountOfTime, // max item problem, find fix
    SecondLife,
    damageIncreaseWhenRage

}
