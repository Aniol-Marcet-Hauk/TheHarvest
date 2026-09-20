using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class EnemyUnderGroundRodent : MonoBehaviour
{
    [SerializeField] private float speed,damage;
    [SerializeField] private float distanceAttackPlayer, randomAngle, timeChangeDir, trndChangeDir, coolDownTime, coolDownRandomness;
    private Enemy enScript;
    private NavMeshAgent agent;
    private Transform player;
    bool coolDown,hasHit,isAttacking,startedAttack;
    void Start()
    {
        enScript = transform.GetComponent<Enemy>();
        agent = transform.GetComponent<NavMeshAgent>();
        StartCoroutine(mov());
        player = GameManager.gameManager.PLAYER.transform;
        coolDown = true;
    }
    IEnumerator startCooldown()
    {
        yield return new WaitForSeconds(coolDownTime + Random.Range(-coolDownRandomness, coolDownRandomness));
        coolDown = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && hasHit == false && isAttacking == true)
        {
            hasHit = true;

            GameManager.gameManager.jugadorAtacat(damage);
        }
    }

    void Update()
    {
        if (!enScript.death)//add a bool to enemy script that checks for a Stun2???
        {
            if (enScript.enemyLife._vida == 0f)
            {
                enScript.death = true;
                agent.isStopped = true;
                StartCoroutine(enScript.Mort());
            }
            else
            {
                Moviment();

            }
        }
    }


    void Moviment()
    {
        if (Vector3.Distance(player.position, transform.position) <= distanceAttackPlayer && coolDown == false)
        {
            coolDown = true;
            startedAttack = true;
            agent.SetDestination(player.position);
        }
        if(startedAttack == true)
        {
            agent.SetDestination(player.position);
        }
        if (coolDown == true && Vector3.Distance(player.position, transform.position) <= 2f)
        {
            startedAttack = false;
            agent.isStopped = true;
            StartCoroutine("enAttack");

        }
    }
    IEnumerator mov()
    {
        
        float rand = Random.Range(-randomAngle * .5f, randomAngle * .5f);

        Quaternion randomizedDirection = Quaternion.Euler(0f, rand, 0f);
        Vector3 RandomDirection = randomizedDirection * transform.forward;
        agent.SetDestination(RandomDirection * 20f + transform.position);

        yield return new WaitForSeconds(timeChangeDir + Random.Range(-trndChangeDir , trndChangeDir));
        StartCoroutine(mov());
    }

    private IEnumerator enAttack()
    {

        
        if (!enScript.death)
        {
            transform.DOLookAt(player.position, 0.3f);

            enScript.anEn.SetBool("run", false);
            agent.isStopped = true;

            enScript.anEn.applyRootMotion = true;
            
            enScript.anEn.Play("C_Combat_IDle");
            yield return new WaitForSeconds(0.35f);
        }


        if (!enScript.death)
        {

            isAttacking = true;
            enScript.anEn.Play("C_Jump_Forward");
            yield return new WaitForSeconds(1.1f);



            
            hasHit = false;
            yield return new WaitForSeconds(0.8f);
            isAttacking = false;
            agent.isStopped = false;
        }

        yield return new WaitForSeconds(coolDownTime + Random.Range(-coolDownRandomness, coolDownRandomness));
        coolDown = false;
    }

}

