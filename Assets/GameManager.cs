using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Cinemachine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set;}
    private void Awake()
    {
        
        if(gameManager != null && gameManager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gameManager = this;
        }

    }

    public HealthClass jugadorVida = new HealthClass(100f,100f);
    public List<GameObject> allWeapons;
    public List<GameObject> allItems;
    public List<GameObject> allUpgrades;//this one might not be necessary

    public static Action lightAttack, heavyAttack;
    public HealthBar HEALTH_BAR;
    //FOR ITEMS AND UPGRADES
    public float playerDamageMultiplier = 1f,playerDamageEnemiesInRageMode = 1f;
    public int ItemPickUpAmount = 1;
    public bool explodeWhenMelee;
    public Animator Bomb;
    public List<UpgradesList> upgradesList = new List<UpgradesList>();
    public PlayerMain PLAYER;
    public float RAGE = 0f;
    public int coins = 0;

    [Space]
    public GameObject loadingScene;
    public GameObject bloodSplatter;
    public float bloodTime = 2f;
    public CinemachineBasicMultiChannelPerlin cam;

    public NoiseSettings noiseProfile1,noiseProfile2;
    [SerializeField] private AudioSource generalAudio,playerDamaged;
    [Space]
    public TMP_Text upgradeShow;
    [Space]
    [SerializeField] private GameObject WINScreen, textWin;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        playerDamageMultiplier = 1f;
        //GameObject GO = GameObject.Find("HealthBarPlayer");
        //HEALTH_BAR = GO.GetComponent<HealthBar>();
        PLAYER = FindObjectOfType<PlayerMain>();
        HEALTH_BAR.SetMaxHealth(jugadorVida._vidaMax);
        HEALTH_BAR.SetHealth(jugadorVida._vida);
        
        //allWeapons = CreateList("Assets/ALL_W_I_U/Armes");
        //allItems = CreateList("Assets/ALL_W_I_U/Items");
        //allUpgrades = CreateList("Assets/ALL_W_I_U/Upgrades");
    }
    public void jugadorAtacat(float _mal)
    {
        jugadorVida.CanviarVida(-_mal*playerDamageMultiplier);
        HEALTH_BAR.SetHealth(gameManager.jugadorVida._vida);
        PLAYER.invincibilityAfterHit(1f);
        if(_mal > 0)
        {
            if(PLAYER.rageModeON == false)
            {
                RAGE += _mal * 0.3f;
            }
            
            foreach (UpgradesList i in upgradesList)
            {
                i.upgrade._OnGetHit(PLAYER, _mal, i.strength);

            }
            StartCoroutine(_ProcessShake(3.2f, 1.5f, .015f));
            playerDamaged.Play();
        }

        
    }

    //THINK ABOUT THE KNOCKBACKTHING


    /*public void TakeKnockBack(Vector3 _pos, float _distanceKnockBack, float _speedKnockBack)//add distance to kncokbackThrough here and speed of knockbackHere
    {
        if (_pos == Vector3.zero)
        {
            return;

        }

        Vector3 endPos = (_pos - transform.position).normalized * _distanceKnockBack * -1f;
        endPos.y = 0f;
        RaycastHit ray;
        if (Physics.Raycast(transform.position, endPos - transform.position, out ray, _distanceKnockBack, knockBackLayers))
        {
            transform.DOMove(transform.position + endPos * ray.distance / DistanceKnockBack, _speedKnockBack * ray.distance / DistanceKnockBack).OnComplete(() => anEn.Play("finishKnockBack"));
        }
        transform.DOMove(transform.position + endPos, _speedKnockBack).OnComplete(() => anEn.Play("finishKnockBack"));

    }*/
    public void OnHitEnemy(Enemy _enemy)
    {
        foreach (UpgradesList i in upgradesList)
        {
            i.upgrade._OnHitEnemy(PLAYER, _enemy, i.strength);

        }
    }


    /*public List<GameObject> CreateList(string _locationInAssets)
    {
        List<GameObject> _finalList = new List<GameObject>();
       
        string[] _list = AssetDatabase.FindAssets("t:Prefab", new string[] { _locationInAssets });
        foreach (var i in _list)
        {
            
            var _path = AssetDatabase.GUIDToAssetPath(i);
            GameObject _go = AssetDatabase.LoadAssetAtPath<GameObject>(_path);
            if(_go != null)
            {
                _finalList.Add(_go);
            }

            

        }
        return _finalList;
       
    }*/
    public GameObject chooseRandWeapon(List<GameObject> weapons)
    {
        int i = UnityEngine.Random.Range(0, weapons.Count);
        //this should be more complex
        return weapons[i];
    }
    public GameObject chooseRandItem(List<GameObject> items)
    {
        int i = UnityEngine.Random.Range(0, items.Count);
        //this should be more complex
        return items[i];
    }
    public GameObject chooseRandUpgrade(List<GameObject> upgrades)
    {
        int i = UnityEngine.Random.Range(0, upgrades.Count);
        //this should be more complex
        return upgrades[i];
    }



    public IEnumerator _ProcessShake( float shakeAmp = 5f,float shakeInt = 5f, float shakeTiming = 0.1f)
    {

        cam.m_AmplitudeGain = shakeAmp;
        cam.m_FrequencyGain = shakeInt;
       
        cam.m_NoiseProfile = noiseProfile1;
        yield return new WaitForSeconds(shakeTiming);
        
        cam.m_AmplitudeGain = .6f;
        cam.m_FrequencyGain = .6f;
        cam.m_NoiseProfile = noiseProfile2;
    }

    public void camSet(CinemachineBasicMultiChannelPerlin _c)
    {
        if(_c == null)
        {
            return;
        }
        cam = _c;
    }
    public void PlayGeneralAudio(AudioClip _aud)
    {
        generalAudio.clip = _aud;
        generalAudio.Play();
    }
    public IEnumerator UpgradeShow(string up)
    {

        upgradeShow.text = up;
        upgradeShow.gameObject.SetActive(true);
        upgradeShow.transform.DOPunchScale(upgradeShow.transform.localScale * 1.2f, .2f);
        yield return new WaitForSecondsRealtime(1.5f);
        Debug.Log("2");
        upgradeShow.text = "";
        upgradeShow.gameObject.SetActive(false);
    }
    public void WIN()
    {
        StartCoroutine(WinPlay());
    }
    private IEnumerator WinPlay()
    {
        yield return new WaitForSeconds(2.6f);
        WINScreen.SetActive(true);
        Vector3 sc = textWin.transform.localScale;
        textWin.transform.localScale = Vector3.zero;
        textWin.transform.DOScale(sc, .7f);
        yield return new WaitForSeconds(35f);
        textWin.transform.DOScale(Vector3.zero, 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);

    }
}

