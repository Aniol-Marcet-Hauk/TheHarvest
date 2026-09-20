using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySurrounderBasic : MonoBehaviour
{

    [SerializeField] private Enemy enScript;

    [SerializeField] private float speed, speedSurrounding, distance,
        randomExtradistance, attackSpeed, attackSpeedRandomisation,
        distanceStartAttacking, distanceAttackPlayer, durationAttack;
    private Transform player;
    private NavMeshAgent thisAgent;
    private bool isAttacking,isAttackingCheck;
    private int direction;
    [Header("Audio")]
    [SerializeField] private AudioClip[] ogreSounds;
    [SerializeField] private float wtAudio, randwtAudio;

    void Start()
    {
        enScript.AnEn.SetInteger("mode", 1);
        direction = Random.Range(1, 3);
        speedSurrounding += Random.Range(-.3f, .3f);
        isAttacking = false;
        isAttackingCheck = false;
        player = GameManager.gameManager.PLAYER.transform;
        thisAgent = transform.GetComponent<NavMeshAgent>();
        distance += Random.Range(-randomExtradistance, randomExtradistance);
        StartCoroutine(ChangeDirectionWalking());
        StartCoroutine(primeraEsperaRandom());
        StartCoroutine(SoundWolf());
    }

    // Update is called once per frame
    void Update()
    {
        if (!enScript.Death)
        {
            if (enScript.EnemyLife._vida == 0)
            {
                enScript.Death = true;
                enScript.AnEn.speed = 0;
                thisAgent.isStopped = true;
                StartCoroutine(enScript.Mort());
            }
            else
            {
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                EnMovement(); 
            }

        }
        
    }
    
    void EnMovement()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceFromPlayer< distanceStartAttacking)
        {
            if(distanceFromPlayer > distance)
            {
                enScript.AnEn.SetInteger("mode", 1);
                RunToPlayer();
                isAttacking = false;
                isAttackingCheck = false;
                return;
            }
            if(isAttacking == false)
            {
                enScript.AnEn.SetInteger("mode", 2);
                Surround();
                return;
            }
            if(distanceFromPlayer > distanceAttackPlayer && isAttackingCheck == false)
            {
                enScript.AnEn.SetInteger("mode", 1);
                RunToPlayer();
                return;
            }
            if(isAttackingCheck == false)
            {
                
                isAttackingCheck = true;
                thisAgent.isStopped = true;
                StartCoroutine(Atac());
            }
            else if(thisAgent.isStopped == false)
            {
                enScript.AnEn.SetInteger("mode", 4);
                RunAway(distanceFromPlayer);
            }

            return;
        }
        isAttacking = false;
    }

    void RunToPlayer()
    {
        
        thisAgent.speed = speed;
        thisAgent.SetDestination(player.position);
    }
    void RunAway(float d)
    {

        if(d < distance-.4f)
        {

            Vector3 playerDirection = transform.position - player.position;
            thisAgent.SetDestination(transform.position + playerDirection);
            return;
        }
        isAttacking = false;
        isAttackingCheck = false;


    }
    void Surround()
    {
        thisAgent.speed = speedSurrounding;
        if (direction == 0)
        {
            direction = Random.Range(1, 3);
        }
        if(direction == 1)
        {
            enScript.AnEn.SetBool("Mirror", false);
            thisAgent.SetDestination(transform.position + transform.right);
        }
        else
        {
            enScript.AnEn.SetBool("Mirror", true);
            thisAgent.SetDestination(transform.position -transform.right);
        }
    }
    
    IEnumerator Atac()
    {
        //enScript.anEn.Play("attack");
        //enScript.anEn.SetBool("IsInteracting", true);
        enScript.AnEn.SetInteger("mode", 3);
        direction = 0;
        yield return new WaitForSeconds(durationAttack);
        enScript.AnEn.SetInteger("mode", 4);
        thisAgent.isStopped = false;
 
        
        //enScript.anEn.SetBool("IsInteracting", false);

    }
    IEnumerator primeraEsperaRandom()
    {
        yield return new WaitForSeconds(Random.Range(0f,1.5f));
        StartCoroutine(triaQuanAtaca());
    }
    IEnumerator triaQuanAtaca()
    {

        yield return new WaitForSeconds(attackSpeed + Random.Range(-attackSpeedRandomisation, attackSpeedRandomisation));
        isAttacking = true;
        yield return new WaitUntil(() => !isAttacking);
        StartCoroutine(triaQuanAtaca());
    }
    IEnumerator ChangeDirectionWalking()
    {
        yield return new WaitForSeconds(Random.Range(7f, 10f));
        direction = Random.Range(1, 3);
        StartCoroutine(ChangeDirectionWalking());
        
    }

    private IEnumerator SoundWolf()
    {

        yield return new WaitForSeconds(Random.Range(wtAudio, wtAudio + randwtAudio));
        if (!enScript.Death)
        {
            int r = Random.Range(0, ogreSounds.Length);
            enScript.TypicalSound.clip = ogreSounds[r];
            enScript.TypicalSound.Play();
            StartCoroutine(SoundWolf());
        }
    }

}
