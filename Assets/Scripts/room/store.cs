using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class store : MonoBehaviour
{
    [SerializeField] private Transform m_Weapon;
    [SerializeField] private Transform m_ItemOrUpgrade;
    [SerializeField] private Transform m_ItemOrUpgrade1;
    [SerializeField] private Transform m_ItemOrUpgrade2;
    [SerializeField] private payForItem m_WeaponPay;
    [SerializeField] private payForItem m_ItemOrUpgradePay;
    [SerializeField] private payForItem m_ItemOrUpgrade1Pay;
    [SerializeField] private payForItem m_ItemOrUpgrade2Pay;
    private void Start()
    {

        GameObject weaponObject = GameManager.gameManager.chooseRandWeapon(GameManager.gameManager.allWeapons);
        Instantiate(weaponObject, m_Weapon.position, weaponObject.transform.rotation);
        m_ItemOrUpgradePay.price = InstantiateUpdaterOrItem(m_ItemOrUpgrade);
        m_ItemOrUpgradePay.text.text = m_ItemOrUpgradePay.price.ToString();
        m_ItemOrUpgrade1Pay.price = InstantiateUpdaterOrItem(m_ItemOrUpgrade1);
        m_ItemOrUpgrade1Pay.text.text = m_ItemOrUpgrade1Pay.price.ToString();
        m_ItemOrUpgrade2Pay.price = InstantiateUpdaterOrItem(m_ItemOrUpgrade2);
        m_ItemOrUpgrade2Pay.text.text = m_ItemOrUpgrade2Pay.price.ToString();
    }
    private int InstantiateUpdaterOrItem(Transform _pos)
    {
        GameObject upgradeOrItem;
        int price = 0;
        if (Random.Range(0, 2) == 0)
        {
            upgradeOrItem = GameManager.gameManager.chooseRandUpgrade(GameManager.gameManager.allUpgrades);
            price = upgradeOrItem.GetComponent<UpgradePickup>().price;
        }
        else
        {
            upgradeOrItem = GameManager.gameManager.chooseRandItem(GameManager.gameManager.allItems);
            price = upgradeOrItem.GetComponent<ItemHolder>().price;
        }
        Instantiate(upgradeOrItem, _pos.position, upgradeOrItem.transform.rotation);
        return price;
        
    }


}
