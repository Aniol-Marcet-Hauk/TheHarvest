using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UI_ControlScirpt : MonoBehaviour
{
   
    [SerializeField] private Texture2D cursor;
    //afegir aqui el healthBar
    [SerializeField]
    private RawImage itemImage, itemImageLeft, itemImageRight;
    [SerializeField]
    private RectTransform itemImageCoolDown;
    [SerializeField]
    private TMP_Text countText;
    private int numLeft, numRight;
    private int count;
    private PlayerMain pm;
    [Header("rageBar")]
    [SerializeField]
    private Slider sliderRage;
    [Header("Coins")]
    [SerializeField]private TMP_Text coins;
    [Header("Weapons")]
    private TMP_Text weaponName1, weaponName2;
    [SerializeField] private RawImage WeaponSprite1, WeaponSprite2;
    private int wepNum = 0;
    [Space]
    [SerializeField] private GameObject pauseMenu;
    private void Start()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
        weaponName1 = WeaponSprite1.GetComponentInChildren<TMP_Text>();
        weaponName2 = WeaponSprite2.GetComponentInChildren<TMP_Text>();
    }
    private void Update()
    {

        if(pm == null)
        {
            pm = FindObjectOfType<PlayerMain>();
            if(pm == null)
            {
                return;
            }
            sliderRage.maxValue = pm.maxRage;

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf == false)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            }

        }
        if (pm.itemsL.Count != 0)
        {
            itemImage.texture = pm.itemsL[pm.itemNum].item.imageUI();
            itemImage.color = Color.white;
            if(pm.itemsL[pm.itemNum].amount < 0)
            {
                countText.text = "∞";
            }
            else
            {
                countText.text = pm.itemsL[pm.itemNum].amount.ToString();

            }
            
            numLeft = pm.itemNum - 1;
            if (pm.timeCoolDownOfItem != 0)
            {

                itemImageCoolDown.localScale= Vector3.one * 2;
                itemImageCoolDown.DOScaleX(0, pm.timeCoolDownOfItem);
                pm.timeCoolDownOfItem = 0f;
            }

            if(numLeft < 0)
            {
                numLeft = pm.itemsL.Count - 1;
            }
            

            itemImageLeft.color = Color.white;
            itemImageLeft.texture = pm.itemsL[numLeft].item.imageUI();

            numRight = pm.itemNum + 1;
            if (numRight > pm.itemsL.Count -1)
            {
                numRight = 0;
            }

            
            itemImageRight.color = Color.white;
            itemImageRight.texture = pm.itemsL[numRight].item.imageUI();
            
            
        }
        else
        {
            countText.text = "";
            itemImage.texture = null;
            itemImage.color = Color.clear;
            itemImageLeft.color = Color.clear;
            itemImageRight.color = Color.clear;
        }
        sliderRage.value = GameManager.gameManager.RAGE;
        coins.text = GameManager.gameManager.coins.ToString();
        if(pm.arma2 == null)
        {

            wepNum = 0;
            WeaponSprite2.color = Color.clear;
            WeaponSprite1.texture = pm.arma1.WeaponSprite;
            WeaponSprite1.transform.localScale = Vector3.one * 1.2f;
            weaponName1.text = pm.arma1.armaNom;
            return;
        }
        else if(pm.armaNum == 1 && wepNum != pm.armaNum || pm.armaNum == 1 && pm.arma1.armaNom != weaponName1.text)
        {
            wepNum = 1;
            WeaponSprite1.texture = pm.arma1.WeaponSprite;
            WeaponSprite1.transform.DOScale(Vector3.one * 1.2f, .2f);
            weaponName1.text = pm.arma1.armaNom;
            WeaponSprite2.texture = pm.arma2.WeaponSprite;
            WeaponSprite2.transform.DOScale(Vector3.one, .2f);
            weaponName2.text = pm.arma2.armaNom;
            WeaponSprite2.color = Color.white;
        }
        else if(pm.armaNum == 2 && wepNum != pm.armaNum || pm.armaNum == 2 && pm.arma1.armaNom != weaponName2.text)
        {
            wepNum = 2;
            WeaponSprite2.texture = pm.arma1.WeaponSprite;
            WeaponSprite2.transform.DOScale(Vector3.one * 1.2f, .2f);
            weaponName1.text = pm.arma2.armaNom;
            WeaponSprite1.texture = pm.arma2.WeaponSprite;
            WeaponSprite1.transform.DOScale(Vector3.one, .2f);
            WeaponSprite2.color = Color.white;
            weaponName2.text = pm.arma1.armaNom;
        }
        
        

    }

    public void changeItemImage()
    {
        
    }



}
