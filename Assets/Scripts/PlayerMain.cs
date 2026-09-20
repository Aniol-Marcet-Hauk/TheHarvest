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
    public PlayerAnimation animScript { get; private set; }
    public bool moviment = true;
    public bool p_attacant = false;
    public float rollTime;
    public float rollVelocity;
    public float velocitat;
    public float rotationSpeed = 2f;
    public Rigidbody player;
    private Vector3 InputDirection;
    public Vector3 InputMouseRot;
    public Vector3 rot { get; private set; }
    private float relPos;
    [Header("coses de l'Arma")]
    [Space]
    //private float mal;
    public Armes arma1;
    public Armes arma2;
    public int armaNum;
    public ArmaAgafadorIInstansiador instansiadorArma;
    public LayerMask lm, lmWALL;
    [Header("Items and Upgrades")]
    public List<ItemsList> itemsL = new List<ItemsList>();
    public int itemNum;
    private bool COOLDOWN_BOOL = false;
    public int itemsMaxAmount = 5;
    public float timeCoolDownOfItem = 0f;
    
    [Header("RAGE")]
    public bool rageModeON;
    public float maxRage = 5f;
    public float rageLowerPerSecond = 0.5f;
    // freeflowingAMOUNT:
    private float freeFlowSize = 1f;
    private float freeFlowMoveForward = 1.6f;


    
    private bool inRangeChest = false;

    private bool coolDown, deathHasHappened =false;
    [SerializeField] private CinemachineVirtualCamera c;

    [SerializeField] private GameObject walkingSound;
    private void Start()
    {
   
        player = gameObject.GetComponent<Rigidbody>();
        animScript = gameObject.GetComponentInChildren<PlayerAnimation>();
        instansiadorArma = gameObject.GetComponentInChildren<ArmaAgafadorIInstansiador>();
        
        instansiadorArma.InstanciaArma(arma1);
        itemNum = 0;
        arma1.Enable();
        armaNum = 1;
        //FireAttacks upgrade = new FireAttacks();
        //GameManager.gameManager.upgradesList.Add(new UpgradesList(upgrade, upgrade.giveName(), 3));
        GameManager.gameManager.camSet(c.GetCinemachineComponent< CinemachineBasicMultiChannelPerlin>());
        StartCoroutine(CallUpgrades());
        //itemsL.Add(new ItemsList(healItem, healItem.giveName(), 1));
    }
    
    void Update()
    {
        InputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        InputMouseRot = new Vector3(Input.mousePosition.x,0,Input.mousePosition.y);
        
        moviment = !animScript.an.GetBool("IsInteracting");
        if(InputDirection != Vector3.zero && moviment == true)
        {
            walkingSound.SetActive(true);
        }
        else
        {
            walkingSound.SetActive(false);
        }
        //arma1.mal = this.mal;
        if(GameManager.gameManager.RAGE >= maxRage && rageModeON == false)
        {
            rageModeON = true;
            StartCoroutine(RAGEMODE());
        }
        if(arma1 == null)
        {
            instansiadorArma.destrossarArma();
        }
        if(GameManager.gameManager.jugadorVida._vida <= 0 && rageModeON == false)
        {
            foreach (UpgradesList i in GameManager.gameManager.upgradesList)
            {

                i.upgrade._OnDie(this, i, i.strength);

                
            }
            if(GameManager.gameManager.jugadorVida._vida <= 0 && rageModeON == false && deathHasHappened ==false) 
            {
                deathHasHappened = true;
                StartCoroutine(Die());
            }
            
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && animScript.an.GetBool("IsInteracting") == false)
        {
            StartCoroutine(RollBackDodge());



            
        }
        if (Input.GetKeyDown(KeyCode.Q) && itemsL.Count != 0)
        {
            itemNum -= 1;
            if(itemNum < 0)
            {
                itemNum = itemsL.Count- 1;
            } 
           
        }
        else if (Input.GetKeyDown(KeyCode.E) && itemsL.Count != 0)
        {
            itemNum += 1;
            if (itemNum > itemsL.Count - 1)
            {
                itemNum = 0;
            }

        }
        if (Input.GetKeyDown(KeyCode.F) && inRangeChest == false && COOLDOWN_BOOL == false && itemsL.Count != 0 && itemsL[itemNum] != null)
        {
            foreach (UpgradesList i in GameManager.gameManager.upgradesList)
            {
                i.upgrade._OnUseItem(itemsL[itemNum], i.strength);

            }
            COOLDOWN_BOOL = true;
            //On item throw
            itemsL[itemNum].item._Update(this);
            StartCoroutine(CoolDownTimer(itemsL[itemNum].coolDown));

            itemsL[itemNum].amount -= 1;
            if(itemsL[itemNum].amount == 0)
            {
                itemsL.RemoveAt(itemNum);

                itemNum += 1;
                if (itemNum > itemsL.Count - 1)
                {
                    itemNum = 0;
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2) && armaNum == 1 || Input.GetKeyDown(KeyCode.Alpha1) && armaNum == 2)
        {
            changeWeapon();
        }
        


        Move(Time.fixedDeltaTime);
        if(moviment == true)
        {
            Rotate(Time.deltaTime);
        }
        
        Atac();
        Vector2 animMove = new Vector2(relPos, Vector3.Dot(transform.right, InputDirection)).normalized;
        animScript.an.SetFloat("movimentx", animMove.x);

        
        
        animScript.an.SetFloat("movimenty",animMove.y );
        float _rotating = Vector3.Dot(transform.right, rot.normalized);
        animScript.an.SetFloat("rotating", _rotating);

        if(itemsL.Count == 0)
        {
            COOLDOWN_BOOL = false;
        }
    }
    

    void Atac()
    {
        if(arma1 == null || coolDown == true)
        {
            return;
        }
        if(animScript.an.GetBool("IsInteracting") == false ||animScript.GetPermetreCombo()== true )
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                transform.rotation = Quaternion.LookRotation(rot);
                //transform.rotation = Quaternion.LookRotation(rot);
                GameManager.lightAttack();
                if (arma1.usesFreeFlow == true)
                {
                    StartCoroutine(GiraIMiraElEnemic(true,freeFlowMoveForward,freeFlowSize,0.2f));
                }


                //animScript.PlayerTargetAn(arma1.atacFluix, true);

            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                transform.rotation = Quaternion.LookRotation(rot);
                GameManager.heavyAttack();
                if(arma1.usesFreeFlow == true)
                {
                    StartCoroutine(GiraIMiraElEnemic(true, freeFlowMoveForward, freeFlowSize,0.2f));
                }
               
                
                //animScript.PlayerTargetAn(arma1.atacFort, true);
            }
        }
        

        
    }

    public IEnumerator GiraIMiraElEnemic(bool doMove,float _moveForward, float size,float timeItTakes)
    {

        //canviar-ho a sphere cast no all per problemes quan estàn en linia
        RaycastHit info; 
        
        
        if (Physics.SphereCast(transform.position - transform.forward  * size*1.1f, size, rot, out info,_moveForward, lm))
        {

            if (!info.collider.transform.GetComponent<Enemy>().GetSTUN())
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

        relPos = Vector3.Dot(transform.forward, InputDirection);
        
        
        float i = (relPos * 0.15f + 1f);
        //Vector3 cameraInputDir = InputDirection.z * Camera.main.transform.forward.normalized + InputDirection.x * Camera.main.transform.right.normalized;
        Vector3 vel =InputDirection * velocitat * i;

        if (moviment == true)
        {


            /*if(Physics.Raycast(transform.position, InputDirection.x * Vector3.right, 0.5f, lmWALL))
            {
                vel.x = 0;
            }
            if (Physics.Raycast(transform.position, InputDirection.z*Vector3.forward,0.5f, lmWALL))
            {


                vel.z = 0;
            }*/
            Collider[] coll = Physics.OverlapSphere(transform.position, 0.5f, lmWALL);
            //RaycastHit[] ray = Physics.SphereCastAll(transform.position, 0.5f, transform.forward, 0f, lmWALL);
            if (coll.Length != 0)
            {
                foreach(Collider c in coll)
                {
                    
                    //la y s'haurà de canviar perquè estigui al peu del personatge

                    
                    Vector3 closestPoint = (c.ClosestPoint(transform.position) - transform.position).normalized;
                    float finish = Vector3.Dot(InputDirection, closestPoint);
                    if (finish > 0)
                    {
                        vel -= closestPoint * finish*velocitat*i;
                    }
                }
            }
            float timeScale = (float)(1 + (1.0 - Time.timeScale));
            player.velocity = vel * timeScale;
        }
        else
        {
            player.velocity += vel * 0.1f;
            
        }






        
      
    }
    public void Rotate(float deltaT)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);

        rot = new Vector3(InputMouseRot.x - pos.x, 0, InputMouseRot.z - pos.y);

        
        float rotation = Mathf.Atan2(rot.x, rot.z) * Mathf.Rad2Deg;
        Quaternion inputRotation = Quaternion.AngleAxis(rotation, Vector3.up);
       
        player.rotation = Quaternion.Slerp(transform.rotation, inputRotation, rotationSpeed * deltaT);

        
        
    }

    IEnumerator RollBackDodge()
    {

        //segurament crear animacions per roll a cada direccio i fer-ho aixi

        if (InputDirection != Vector3.zero)
        {
            player.rotation = Quaternion.LookRotation(InputDirection);
        }
        yield return new WaitForFixedUpdate();

        animScript.PlayerTargetAn("Roll", true);

        if (InputDirection != Vector3.zero)
        {
            player.rotation = Quaternion.LookRotation(InputDirection);
        }

        //StartCoroutine(Roll2(gameObject.GetComponent<Collider>(), 0.1f, 0.85f));

    }

    

    public void Combos()
    {

    }

    
    
    private IEnumerator Die()
    {

        animScript.PlayerTargetAn("death", true);
        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene(0);
        
        // Disable character controls
        // Trigger game over logic
    }
    public void ItemInteract()
    {
        animScript.PlayerTargetAn("ItemInt",false);
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
    public IEnumerator RAGEMODE()
    {
        
        float vel = velocitat;
        yield return new WaitForSeconds(0.01f);
        GameManager.gameManager.playerDamageEnemiesInRageMode = 1.5f;
        velocitat = 9f;
        freeFlowMoveForward = 10f;
        freeFlowSize = 2f;
        while (GameManager.gameManager.RAGE > 0)
        {
            
            GameManager.gameManager.RAGE -= rageLowerPerSecond * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
           
        }
        GameManager.gameManager.playerDamageEnemiesInRageMode = 1f;
        freeFlowMoveForward = 0.9f;
        freeFlowSize = 1f;
        velocitat = vel;
        Time.timeScale = 1f;
        rageModeON = false;


    }
    IEnumerator CoolDownTimer(float time)
    {
       
        timeCoolDownOfItem = time;
        yield return new WaitForSeconds(time);
        COOLDOWN_BOOL = false;
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
    public IEnumerator invincibilityAfterHit(float sec)
    {
        animScript.activarIframesRoll();
        yield return new WaitForSeconds(sec);
        animScript.apagarIframesRoll();
    }


    private void changeWeapon()
    {
        if(arma1 == null || arma2 == null|| !moviment)
        {
            return;
        }
        Armes a = arma1;
        arma1 = arma2;
        arma2 = a;
        instansiadorArma.destrossarArma();
        instansiadorArma.InstanciaArma(arma1);
        arma2.Disable();
        arma1.Enable();

        if (armaNum == 1)
        {
            armaNum = 2;
    
            return;
        }
  
        armaNum = 1;
    }
    public void setInRangeChest(bool chest)
    {
        inRangeChest = chest;
    }
    public void SetBoolCoolDown(bool c)
    {
        coolDown = c;
    }
}

