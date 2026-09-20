using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class store : MonoBehaviour
{
    [SerializeField] private Transform weapon, itemOrUpgrade, itemOrUpgrade1, itemOrUpgrade2;
    [SerializeField] private payForItem weaponPay, itemOrUpgradePay, itemOrUpgrade1Pay, itemOrUpgrade2Pay;
    private void Start()
    {

        GameObject _Weapon = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
        Instantiate(_Weapon, weapon.position, _Weapon.transform.rotation);
        itemOrUpgradePay.price = InstantiateUpdaterOrItem(itemOrUpgrade);
        itemOrUpgradePay.text.text = itemOrUpgradePay.price.ToString();
        itemOrUpgrade1Pay.price = InstantiateUpdaterOrItem(itemOrUpgrade1);
        itemOrUpgrade1Pay.text.text = itemOrUpgrade1Pay.price.ToString();
        itemOrUpgrade2Pay.price = InstantiateUpdaterOrItem(itemOrUpgrade2);
        itemOrUpgrade2Pay.text.text = itemOrUpgrade2Pay.price.ToString();
    }
    private int InstantiateUpdaterOrItem(Transform _pos)
    {
        GameObject _UorI;
        int _price = 0;
        if (Random.Range(0, 2) == 0)
        {
            _UorI= GameManager.gameManager.chooseRandUpgrade(GameManager.gameManager.allUpgrades);
            _price = _UorI.GetComponent<UpgradePickup>().price;
        }
        else
        {
            _UorI = GameManager.gameManager.chooseRandItem(GameManager.gameManager.allItems);
            _price = _UorI.GetComponent<ItemHolder>().price;
        }
        Instantiate(_UorI, _pos.position, _UorI.transform.rotation);
        return _price;
        
    }


}
