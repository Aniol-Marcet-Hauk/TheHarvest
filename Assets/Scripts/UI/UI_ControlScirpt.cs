using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UI_ControlScirpt : MonoBehaviour
{
   
    [SerializeField] private Texture2D m_Cursor;
    //afegir aqui el healthBar
    [SerializeField]
    private RawImage m_ItemImage, m_ItemImageLeft, m_ItemImageRight;
    [SerializeField]
    private RectTransform m_ItemImageCoolDown;
    [SerializeField]
    private TMP_Text m_CountText;
    private int m_NumLeft, m_NumRight;
    private int m_Count;
    private PlayerMain m_Pm;
    [Header("rageBar")]
    [SerializeField]
    private Slider m_SliderRage;
    [Header("Coins")]
    [SerializeField]private TMP_Text m_Coins;
    [Header("Weapons")]
    private TMP_Text m_WeaponName1, m_WeaponName2;
    [SerializeField] private RawImage m_WeaponSprite1, m_WeaponSprite2;
    private int m_WepNum = 0;
    [Space]
    [SerializeField] private GameObject m_PauseMenu;
    private void Start()
    {
        Cursor.SetCursor(m_Cursor, Vector2.zero, CursorMode.ForceSoftware);
        m_WeaponName1 = m_WeaponSprite1.GetComponentInChildren<TMP_Text>();
        m_WeaponName2 = m_WeaponSprite2.GetComponentInChildren<TMP_Text>();
    }
    private void Update()
    {
        if (!EnsurePlayerReference())
        {
            return;
        }

        HandlePauseToggle();
        UpdateItemsUi();
        UpdateRageAndCoinsUi();
        UpdateWeaponsUi();
    }

    private bool EnsurePlayerReference()
    {
        if (m_Pm == null)
        {
            m_Pm = FindObjectOfType<PlayerMain>();
            if (m_Pm == null)
            {
                return false;
            }
            m_SliderRage.maxValue = m_Pm.maxRage;
        }

        return true;
    }

    private void HandlePauseToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_PauseMenu.activeSelf == false)
            {
                Time.timeScale = 0;
                m_PauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                m_PauseMenu.SetActive(false);
            }
        }
    }

    private void UpdateItemsUi()
    {
        if (m_Pm.itemsL.Count != 0)
        {
            m_ItemImage.texture = m_Pm.itemsL[m_Pm.itemNum].item.imageUI();
            m_ItemImage.color = Color.white;
            if (m_Pm.itemsL[m_Pm.itemNum].amount < 0)
            {
                m_CountText.text = "∞";
            }
            else
            {
                m_CountText.text = m_Pm.itemsL[m_Pm.itemNum].amount.ToString();
            }

            m_NumLeft = m_Pm.itemNum - 1;
            if (m_Pm.timeCoolDownOfItem != 0)
            {
                m_ItemImageCoolDown.localScale = Vector3.one * 2;
                m_ItemImageCoolDown.DOScaleX(0, m_Pm.timeCoolDownOfItem);
                m_Pm.timeCoolDownOfItem = 0f;
            }

            if (m_NumLeft < 0)
            {
                m_NumLeft = m_Pm.itemsL.Count - 1;
            }

            m_ItemImageLeft.color = Color.white;
            m_ItemImageLeft.texture = m_Pm.itemsL[m_NumLeft].item.imageUI();

            m_NumRight = m_Pm.itemNum + 1;
            if (m_NumRight > m_Pm.itemsL.Count - 1)
            {
                m_NumRight = 0;
            }

            m_ItemImageRight.color = Color.white;
            m_ItemImageRight.texture = m_Pm.itemsL[m_NumRight].item.imageUI();
        }
        else
        {
            m_CountText.text = "";
            m_ItemImage.texture = null;
            m_ItemImage.color = Color.clear;
            m_ItemImageLeft.color = Color.clear;
            m_ItemImageRight.color = Color.clear;
        }
    }

    private void UpdateRageAndCoinsUi()
    {
        m_SliderRage.value = GameManager.gameManager.RAGE;
        m_Coins.text = GameManager.gameManager.coins.ToString();
    }

    private void UpdateWeaponsUi()
    {
        if (m_Pm.arma2 == null)
        {
            m_WepNum = 0;
            m_WeaponSprite2.color = Color.clear;
            m_WeaponSprite1.texture = m_Pm.arma1.WeaponSprite;
            m_WeaponSprite1.transform.localScale = Vector3.one * 1.2f;
            m_WeaponName1.text = m_Pm.arma1.armaNom;
            return;
        }
        else if (m_Pm.armaNum == 1 && m_WepNum != m_Pm.armaNum || m_Pm.armaNum == 1 && m_Pm.arma1.armaNom != m_WeaponName1.text)
        {
            m_WepNum = 1;
            m_WeaponSprite1.texture = m_Pm.arma1.WeaponSprite;
            m_WeaponSprite1.transform.DOScale(Vector3.one * 1.2f, .2f);
            m_WeaponName1.text = m_Pm.arma1.armaNom;
            m_WeaponSprite2.texture = m_Pm.arma2.WeaponSprite;
            m_WeaponSprite2.transform.DOScale(Vector3.one, .2f);
            m_WeaponName2.text = m_Pm.arma2.armaNom;
            m_WeaponSprite2.color = Color.white;
        }
        else if (m_Pm.armaNum == 2 && m_WepNum != m_Pm.armaNum || m_Pm.armaNum == 2 && m_Pm.arma1.armaNom != m_WeaponName2.text)
        {
            m_WepNum = 2;
            m_WeaponSprite2.texture = m_Pm.arma1.WeaponSprite;
            m_WeaponSprite2.transform.DOScale(Vector3.one * 1.2f, .2f);
            m_WeaponName1.text = m_Pm.arma2.armaNom;
            m_WeaponSprite1.texture = m_Pm.arma2.WeaponSprite;
            m_WeaponSprite1.transform.DOScale(Vector3.one, .2f);
            m_WeaponSprite2.color = Color.white;
            m_WeaponName2.text = m_Pm.arma1.armaNom;
        }
    }

    public void ChangeItemImage()
    {
        
    }

    public void changeItemImage() => ChangeItemImage();



}
