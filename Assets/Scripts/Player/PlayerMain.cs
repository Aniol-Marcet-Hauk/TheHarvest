using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using sistemesArmes;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Cinemachine;
public class PlayerMain : MonoBehaviour
{


    // caldrà reorganitsar per canviar les armes a un lloc diferent...
    //quan tingui temps moure coses de arma a player atack scfipt
    [SerializeField]
    //aquest codi és bastant brut ara mateix s'ha de netejar
    public PlayerAnimation AnimScript { get; private set; }
    public bool Moviment { get; private set; } = true;
    [SerializeField] private bool m_PAttacant = false;
    [SerializeField] private float m_RollTime;
    [SerializeField] private float m_RollVelocity;
    public float Velocitat;
    [SerializeField] private float m_RotationSpeed = 2f;
    public Rigidbody Player;
    private Vector3 m_InputDirection;
    private Vector3 m_InputMouseRot;
    public Vector3 Rot { get; private set; }
    private float m_RelPos;
    [Header("coses de l'Arma")]
    [Space]
    //private float mal;
    public Armes Arma1;
    public Armes Arma2;
    public int ArmaNum;
    public ArmaAgafadorIInstansiador InstansiadorArma;
    [SerializeField] private LayerMask m_Lm, m_LmWALL;
    [Header("Items and Upgrades")]
    public List<ItemsList> ItemsL = new List<ItemsList>();
    public int ItemNum;
    private bool m_CooldownBool = false;
    public int ItemsMaxAmount = 5;
    public float TimeCoolDownOfItem = 0f;

    [Header("RAGE")]
    public bool RageModeON;
    [SerializeField] private float m_MaxRage = 5f;
    [SerializeField] private float m_RageLowerPerSecond = 0.5f;
    // freeflowingAMOUNT:
    private float m_FreeFlowSize = 1f;
    private float m_FreeFlowMoveForward = 1.6f;



    private bool m_InRangeChest = false;

    private bool m_CoolDown, m_DeathHasHappened = false;
    [SerializeField] private CinemachineVirtualCamera m_C;

    [SerializeField] private GameObject m_WalkingSound;
    private void Start()
    {

        Player = gameObject.GetComponent<Rigidbody>();
        AnimScript = gameObject.GetComponentInChildren<PlayerAnimation>();
        InstansiadorArma = gameObject.GetComponentInChildren<ArmaAgafadorIInstansiador>();

        InstansiadorArma.InstanciaArma(Arma1);
        ItemNum = 0;
        Arma1.Enable();
        ArmaNum = 1;
        //FireAttacks upgrade = new FireAttacks();
        //GameManager.gameManager.upgradesList.Add(new UpgradesList(upgrade, upgrade.giveName(), 3));
        GameManager.gameManager.camSet(m_C.GetCinemachineComponent< CinemachineBasicMultiChannelPerlin>());
        StartCoroutine(CallUpgrades());
        //itemsL.Add(new ItemsList(healItem, healItem.giveName(), 1));
    }
    
    void Update()
    {
        m_InputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        m_InputMouseRot = new Vector3(Input.mousePosition.x,0,Input.mousePosition.y);

        Moviment = !AnimScript.an.GetBool("IsInteracting");
        if(m_InputDirection != Vector3.zero && Moviment == true)
        {
            m_WalkingSound.SetActive(true);
        }
        else
        {
            m_WalkingSound.SetActive(false);
        }
        //arma1.mal = this.mal;
        if(GameManager.gameManager.RAGE >= m_MaxRage && RageModeON == false)
        {
            RageModeON = true;
            StartCoroutine(RageModeRoutine());
        }
        if(Arma1 == null)
        {
            InstansiadorArma.DestrossarArma();
        }
        if(GameManager.gameManager.jugadorVida._vida <= 0 && RageModeON == false)
        {
            foreach (UpgradesList i in GameManager.gameManager.upgradesList)
            {

                i.upgrade._OnDie(this, i, i.strength);


            }
            if(GameManager.gameManager.jugadorVida._vida <= 0 && RageModeON == false && m_DeathHasHappened ==false)
            {
                m_DeathHasHappened = true;
                StartCoroutine(Die());
            }

        }

        if (Input.GetKeyDown(KeyCode.Space) && AnimScript.an.GetBool("IsInteracting") == false)
        {
            StartCoroutine(RollBackDodge());




        }
        if (Input.GetKeyDown(KeyCode.Q) && ItemsL.Count != 0)
        {
            ItemNum -= 1;
            if(ItemNum < 0)
            {
                ItemNum = ItemsL.Count- 1;
            }

        }
        else if (Input.GetKeyDown(KeyCode.E) && ItemsL.Count != 0)
        {
            ItemNum += 1;
            if (ItemNum > ItemsL.Count - 1)
            {
                ItemNum = 0;
            }

        }
        if (Input.GetKeyDown(KeyCode.F) && m_InRangeChest == false && m_CooldownBool == false && ItemsL.Count != 0 && ItemsL[ItemNum] != null)
        {
            foreach (UpgradesList i in GameManager.gameManager.upgradesList)
            {
                i.upgrade._OnUseItem(ItemsL[ItemNum], i.strength);

            }
            m_CooldownBool = true;
            //On item throw
            ItemsL[ItemNum].item._Update(this);
            StartCoroutine(CoolDownTimer(ItemsL[ItemNum].coolDown));

            ItemsL[ItemNum].amount -= 1;
            if(ItemsL[ItemNum].amount == 0)
            {
                ItemsL.RemoveAt(ItemNum);

                ItemNum += 1;
                if (ItemNum > ItemsL.Count - 1)
                {
                    ItemNum = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && ArmaNum == 1 || Input.GetKeyDown(KeyCode.Alpha1) && ArmaNum == 2)
        {
            ChangeWeapon();
        }



        Move(Time.fixedDeltaTime);
        if(Moviment == true)
        {
            Rotate(Time.deltaTime);
        }

        Atac();
        Vector2 animMove = new Vector2(m_RelPos, Vector3.Dot(transform.right, m_InputDirection)).normalized;
        AnimScript.an.SetFloat("movimentx", animMove.x);



        AnimScript.an.SetFloat("movimenty",animMove.y );
        float _rotating = Vector3.Dot(transform.right, Rot.normalized);
        AnimScript.an.SetFloat("rotating", _rotating);

        if(ItemsL.Count == 0)
        {
            m_CooldownBool = false;
        }
    }
    

    void Atac()
    {
        if(Arma1 == null || m_CoolDown == true)
        {
            return;
        }
        if(AnimScript.an.GetBool("IsInteracting") == false ||AnimScript.GetPermetreCombo()== true )
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                transform.rotation = Quaternion.LookRotation(Rot);
                //transform.rotation = Quaternion.LookRotation(Rot);
                GameManager.lightAttack();
                if (Arma1.usesFreeFlow == true)
                {
                    StartCoroutine(GiraIMiraElEnemic(true,m_FreeFlowMoveForward,m_FreeFlowSize,0.2f));
                }


                //AnimScript.PlayerTargetAn(Arma1.atacFluix, true);

            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                transform.rotation = Quaternion.LookRotation(Rot);
                GameManager.heavyAttack();
                if(Arma1.usesFreeFlow == true)
                {
                    StartCoroutine(GiraIMiraElEnemic(true, m_FreeFlowMoveForward, m_FreeFlowSize,0.2f));
                }


                //AnimScript.PlayerTargetAn(Arma1.atacFort, true);
            }
        }
        

        
    }

    public IEnumerator GiraIMiraElEnemic(bool doMove,float _moveForward, float size,float timeItTakes)
    {

        //canviar-ho a sphere cast no all per problemes quan estàn en linia
        RaycastHit info; 
        
        
        if (Physics.SphereCast(transform.position - transform.forward  * size*1.1f, size, Rot, out info,_moveForward, m_Lm))
        {

            if (!info.collider.transform.GetComponent<Enemy>().GetStun())
            {
                yield return new WaitForSeconds(0.05f);
                Vector3 posEn = info.point;
                posEn.y = transform.position.y;
                transform.DOLookAt(posEn, timeItTakes,AxisConstraint.None,null);
                
                if(doMove == true)
                {
                    transform.DOMove(MovePositionFreeFlow(posEn), timeItTakes);
                }
               
                
                    
                
             
                
                //transform.position = MovePositionFreeFlow(posEn);
            }

        }
    }
    
    private Vector3 MovePositionFreeFlow(Vector3 _positionEn)
    {
        Vector3 check = (transform.position - _positionEn).normalized*.6f;
        Vector3 endP = check+_positionEn;
        endP.y = transform.position.y;
        return endP;
    }
    void Move(float deltaT)
    {

        m_RelPos = Vector3.Dot(transform.forward, m_InputDirection);
        
        
        float i = (m_RelPos * 0.15f + 1f);
        //Vector3 cameraInputDir = InputDirection.z * Camera.main.transform.forward.normalized + InputDirection.x * Camera.main.transform.right.normalized;
        Vector3 vel = m_InputDirection * Velocitat * i;

        if (Moviment == true)
        {


            /*if(Physics.Raycast(transform.position, InputDirection.x * Vector3.right, 0.5f, lmWALL))
            {
                vel.x = 0;
            }
            if (Physics.Raycast(transform.position, InputDirection.z*Vector3.forward,0.5f, lmWALL))
            {


                vel.z = 0;
            }*/
            Collider[] coll = Physics.OverlapSphere(transform.position, 0.5f, m_LmWALL);
            //RaycastHit[] ray = Physics.SphereCastAll(transform.position, 0.5f, transform.forward, 0f, lmWALL);
            if (coll.Length != 0)
            {
                foreach(Collider c in coll)
                {
                    
                    //la y s'haurà de canviar perquè estigui al peu del personatge

                    
                    Vector3 closestPoint = (c.ClosestPoint(transform.position) - transform.position).normalized;
                    float finish = Vector3.Dot(m_InputDirection, closestPoint);
                    if (finish > 0)
                    {
                        vel -= closestPoint * finish * Velocitat * i;
                    }
                }
            }
            float timeScale = (float)(1 + (1.0 - Time.timeScale));
            Player.velocity = vel * timeScale;
        }
        else
        {
            Player.velocity += vel * 0.1f;
            
        }






        
      
    }
    public void Rotate(float deltaT)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);

        Rot = new Vector3(m_InputMouseRot.x - pos.x, 0, m_InputMouseRot.z - pos.y);

        
        float rotation = Mathf.Atan2(Rot.x, Rot.z) * Mathf.Rad2Deg;
        Quaternion inputRotation = Quaternion.AngleAxis(rotation, Vector3.up);
       
        Player.rotation = Quaternion.Slerp(transform.rotation, inputRotation, m_RotationSpeed * deltaT);

        
        
    }

    IEnumerator RollBackDodge()
    {

        //segurament crear animacions per roll a cada direccio i fer-ho aixi

        if (m_InputDirection != Vector3.zero)
        {
            Player.rotation = Quaternion.LookRotation(m_InputDirection);
        }
        yield return new WaitForFixedUpdate();

        AnimScript.PlayerTargetAn("Roll", true);

        if (m_InputDirection != Vector3.zero)
        {
            Player.rotation = Quaternion.LookRotation(m_InputDirection);
        }

        //StartCoroutine(Roll2(gameObject.GetComponent<Collider>(), 0.1f, 0.85f));

    }

    

    public void Combos()
    {

    }

    
    
    private IEnumerator Die()
    {

        AnimScript.PlayerTargetAn("death", true);
        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene(0);
        
        // Disable character controls
        // Trigger game over logic
    }
    public void ItemInteract()
    {
        AnimScript.PlayerTargetAn("ItemInt",false);
    }

    IEnumerator CallUpgrades()
    {
        yield return new WaitForSeconds(1f);
        foreach (UpgradesList i in GameManager.gameManager.upgradesList)
        {
            i.upgrade._Update(this,i.strength);
        }
        StartCoroutine(CallUpgrades());
    }


    //de moment només fa que s'apropi moltissim a enemics i que corri més rapid, però volem que rage mode fagi molt més(efectes i més mal...)
    //a més a més ara només puja quan li fan mal, pero fare que pugi per moltes més coses(com al fer mal)
    //si mata enemics tmb li puja i si es queda sense vida pero està en aquest mode no li passa res
    //speed s'ha de afegir a l'animador un multiplicador pq les animacions corresponguin a la velocitat
    public IEnumerator RageModeRoutine()
    {
        
        float vel = Velocitat;
        yield return new WaitForSeconds(0.01f);
        GameManager.gameManager.playerDamageEnemiesInRageMode = 1.5f;
        Velocitat = 9f;
        m_FreeFlowMoveForward = 10f;
        m_FreeFlowSize = 2f;
        while (GameManager.gameManager.RAGE > 0)
        {
            
            GameManager.gameManager.RAGE -= m_RageLowerPerSecond * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
           
        }
        GameManager.gameManager.playerDamageEnemiesInRageMode = 1f;
        m_FreeFlowMoveForward = 0.9f;
        m_FreeFlowSize = 1f;
        Velocitat = vel;
        Time.timeScale = 1f;
        RageModeON = false;


    }
    IEnumerator CoolDownTimer(float time)
    {
       
        TimeCoolDownOfItem = time;
        yield return new WaitForSeconds(time);
        m_CooldownBool = false;
    }
    /*public void damage(float _mal)
    {
        mal = _mal;
    }*/
    /*IEnumerator Roll2(Collider coll, float _float1,float _float2)
    {

        yield return new WaitForSeconds(_float1);
        coll.enabled = false;
        yield return new WaitForSeconds(_float2);
        coll.enabled = true;

    }*/
    public IEnumerator InvincibilityAfterHit(float sec)
    {
        AnimScript.EnableRollIframes();
        yield return new WaitForSeconds(sec);
        AnimScript.DisableRollIframes();
    }


    private void ChangeWeapon()
    {
        if(Arma1 == null || Arma2 == null|| !Moviment)
        {
            return;
        }
        Armes a = Arma1;
        Arma1 = Arma2;
        Arma2 = a;
        InstansiadorArma.DestrossarArma();
        InstansiadorArma.InstanciaArma(Arma1);
        Arma2.Disable();
        Arma1.Enable();

        if (ArmaNum == 1)
        {
            ArmaNum = 2;
    
            return;
        }
  
        ArmaNum = 1;
    }

    public void SetInRangeChest(bool chest)
    {
        m_InRangeChest = chest;
    }
    public void SetBoolCoolDown(bool c)
    {
        m_CoolDown = c;
    }

    public PlayerAnimation animScript => AnimScript;
    public Rigidbody player => Player;
    public float velocitat { get => Velocitat; set => Velocitat = value; }
    public bool rageModeON { get => RageModeON; set => RageModeON = value; }
    public float maxRage { get => m_MaxRage; set => m_MaxRage = value; }
    public float rageLowerPerSecond { get => m_RageLowerPerSecond; set => m_RageLowerPerSecond = value; }
    public int itemsMaxAmount { get => ItemsMaxAmount; set => ItemsMaxAmount = value; }
    public List<ItemsList> itemsL => ItemsL;
    public Armes arma1 { get => Arma1; set => Arma1 = value; }
    public Armes arma2 { get => Arma2; set => Arma2 = value; }
    public int armaNum { get => ArmaNum; set => ArmaNum = value; }
    public int itemNum { get => ItemNum; set => ItemNum = value; }
    public float timeCoolDownOfItem { get => TimeCoolDownOfItem; set => TimeCoolDownOfItem = value; }
    public Vector3 rot { get => Rot; set => Rot = value; }
    public ArmaAgafadorIInstansiador instansiadorArma => InstansiadorArma;
    public IEnumerator invincibilityAfterHit(float sec) => InvincibilityAfterHit(sec);
    public void setInRangeChest(bool chest) => SetInRangeChest(chest);
    public IEnumerator RAGEMODE() => RageModeRoutine();
}

