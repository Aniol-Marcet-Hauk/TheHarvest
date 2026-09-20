using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class BossCyclops : MonoBehaviour
{
    private NavMeshAgent agentEn;
    private Animator anEn;
    private Enemy enScript;
    private bool isAttacking;
    [SerializeField] private GameObject lazerDamage;
    [SerializeField] private LineRenderer lazerShow;
    [SerializeField]
    private float attackSpeed, attackSpeedRandomisation, distanceForShortAttack,
        speedLazer,speedLazerSpecific, timeLazerFollow, timeJump, damageWithLazer;
    [SerializeField] private string nameAxeAttackAn, nameLazerAttack1, nameLazerAttack2;
    [SerializeField] private Transform headCyclops,eyePos;
    private Transform player;
    [SerializeField] private float vidaMaxFase2;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private LayerMask lm;
    [SerializeField] private Transform unassigned;
    private bool damagedWithLazerPause = false;
    public Collider damageJump;
    public Room roomB;
    public AudioClip[] bossSounds;
    public AudioSource jumpFall;

    private void Start()
    {
        
        enScript = transform.GetComponent<Enemy>();
        anEn = transform.GetComponentInChildren<Animator>();
        agentEn = transform.GetComponent<NavMeshAgent>();
        player = GameManager.gameManager.PLAYER.transform;
        StartCoroutine(timeAttack());
        StartCoroutine(spawnAllEnemies());
        Room[] allrooms = FindObjectsOfType<Room>();
        foreach (var item in allrooms)
        {
            if(item == roomB)
            {
                continue;
            }
            Destroy(item.gameObject);
        }

    }
    void Update()
    {
        if (!enScript.death&& enScript.enemyLife._vida == 0)
        {
           
            enScript.death = true;
            
            StartCoroutine(enScript.Mort());
            GameManager.gameManager.WIN();
            

        }
        if (isAttacking == true)
        {
            isAttacking = anEn.GetBool("isAttacking");
            return;
        }
        
        rotate(Time.deltaTime);

    }
    void rotate(float deltaT)
    {
        

        Vector3 rot = new Vector3(player.position.x-transform.position.x, 0, player.position.z-transform.position.z);


        float rotation = Mathf.Atan2(rot.x, rot.z) * Mathf.Rad2Deg;
        Quaternion inputRotation = Quaternion.AngleAxis(rotation, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, inputRotation, rotationSpeed * deltaT);
        float dot = Vector3.Dot(transform.right, rot.normalized);
        if (dot > 0.1f)
        {

            anEn.SetInteger("RightLeft", 1);
        }
        else if (dot <-0.1f)
        {
            anEn.SetInteger("RightLeft", -1);
        }
        else
        {
            anEn.SetInteger("RightLeft", 0);
        }
    }
    IEnumerator timeAttack()
    {
        yield return new WaitUntil(() => !isAttacking);
        anEn.transform.localPosition = Vector3.up * (-1);
        yield return new WaitForSeconds(attackSpeed + Random.Range(0, attackSpeedRandomisation));
        isAttacking = true;
        
        if(enScript.death == false)
        {
            ChooseAttack();
        }
       
        
        
        yield return new WaitForSeconds(.3f);
        StartCoroutine(timeAttack());
    }
    private void ChooseAttack()
    {
        Debug.Log("A");
        anEn.SetBool("isAttacking",true);
        int chooseAttack = Random.Range(1, 3);
        float distance;
        distance = Vector3.Distance(transform.position, player.position);
        if(distance < distanceForShortAttack)
        {
            chooseAttack = Random.Range(0, 3);
        }
        
        int r = Random.Range(0, bossSounds.Length);
        enScript.typicalSound.clip = bossSounds[r];
        enScript.typicalSound.Play();
        StartCoroutine(SoundCyclops());
        
        switch (chooseAttack)
        {
            case 0:
                stickAttack();
                
                break;
            case 1:
               
                jumpAttack("jumpAn");
                //lazer attack 1

                break;
            case 2:
                followLazerAttack2();
                //lazer attack 2
                break;
           
            
            default:
                followLazerAttack2();
                break;
        }
    }

    
    void stickAttack()
    {
        agentEn.isStopped = true;
        anEn.applyRootMotion = true;
        anEn.Play(nameAxeAttackAn);
    }
    void lazerAttack1()
    {
        agentEn.isStopped = true;
        anEn.applyRootMotion = true;
        anEn.Play(nameLazerAttack1);
        
        //Activate lazer
    }
    void followLazerAttack2()
    {
        agentEn.isStopped = true;
        anEn.Play(nameLazerAttack1);
        StartCoroutine(lazerFollow());
        StartCoroutine(TimeLazerFollow());
    }
    IEnumerator lazerFollow()
    {
        while (isAttacking == true)
        {
            Vector3 rot = player.position - transform.position;


            float rotation = Mathf.Atan2(rot.x, rot.z) * Mathf.Rad2Deg;
            Quaternion inputRotation = Quaternion.AngleAxis(rotation, Vector3.up);

            //transform.rotation = Quaternion.RotateTowards(transform.rotation, inputRotation, speedLazer * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, inputRotation, speedLazer * Time.fixedDeltaTime);
            headCyclops.DOLookAt(player.position, speedLazerSpecific);
            
            
            
            
            
            /*float rot_x = Mathf.Atan2(rot.y, rot.z) * Mathf.Rad2Deg;
           
            Quaternion rotation2 = Quaternion.AngleAxis(rot_x, Vector3.right);

            headCyclops.localRotation= Quaternion.Slerp(headCyclops.rotation, rotation2, speedLazer*Time.deltaTime);*/
            RaycastHit ray;
            if (Physics.Raycast(eyePos.position, eyePos.forward, out ray,lm))
            {
                
         
                lazerShow.SetPosition(1, Vector3.forward * ray.distance);
                if(ray.collider.tag == "Player" && damagedWithLazerPause == false)
                {
                    damagedWithLazerPause = true;
                    StartCoroutine(DamageByLazer());
                }
                //YOU CAN USE THIS FOR THE DAMAGE WITH THE OTHER RAY ASWELL
            }
            yield return new WaitForFixedUpdate();
        }
        headCyclops.DOKill();
        lazerShow.SetPosition(1, Vector3.zero);

    }
    IEnumerator DamageByLazer()
    {
        GameManager.gameManager.jugadorAtacat(damageWithLazer);
       
        yield return new WaitForSeconds(.5f);
        damagedWithLazerPause = false;
    }
    IEnumerator TimeLazerFollow()
    {
        yield return new WaitForSeconds(timeLazerFollow);
        anEn.SetBool("isAttacking", false);
        isAttacking = false;
    }


    void jumpAttack(string anString)
    {
        agentEn.isStopped = true;
        anEn.Play(anString);
        Vector3 v = player.position;
        v.y = transform.position.y;
        transform.DOJump(v, 5, 1, timeJump).SetEase(Ease.InQuad).OnComplete(() => StartCoroutine(DamageJump()));

    }
    IEnumerator jumpFase2()
    {
        int max = Random.Range(1, 5);
        for (int i = 0; i < max; i++)
        {
            jumpAttack("JumpAttack2");
            yield return new WaitForSeconds(timeJump);
        }
        

    }
    IEnumerator DamageJump()
    {
        damageJump.enabled = true;
        jumpFall.Play();
        yield return new WaitForSeconds(.25f);
        damageJump.enabled = false;
    }


    IEnumerator spawnAllEnemies()
    {
        yield return new WaitForSeconds(Random.Range(17f, 45f));
        StartCoroutine(roomB.InstantiateEnemies(Random.Range(0, 13)));
        StartCoroutine(spawnAllEnemies());
    }
    private IEnumerator SoundCyclops()
    {

        yield return new WaitForSeconds(Random.Range(5f,15f));
        if (!enScript.death)
        {
            int r = Random.Range(0, bossSounds.Length);
            enScript.typicalSound.clip = bossSounds[r];
            enScript.typicalSound.Play();
            StartCoroutine(SoundCyclops());
        }
    }
}
