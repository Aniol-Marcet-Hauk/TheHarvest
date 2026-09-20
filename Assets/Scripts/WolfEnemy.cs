using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class WolfEnemy : MonoBehaviour
{
    [SerializeField]
    private float lookDistance = 10f;
    [SerializeField]
    private Transform player;
    public float damage;
    private bool hasHit = false;
    private bool isAttackingPlayer = false, isAttackingP2 = false;
    private Enemy enScript;
    private NavMeshAgent _agent;
    [Header("sounds")]
    [SerializeField] private AudioClip[] barksAndGrowls;
    [SerializeField] private float wtAudio, randwtAudio;
    void Start()
    {
        enScript = transform.GetComponent<Enemy>();
        _agent = GetComponent<NavMeshAgent>();
        player = GameManager.gameManager.PLAYER.transform;
        StartCoroutine(SoundWolf());

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && hasHit == false && isAttackingPlayer && isAttackingP2)
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
                StartCoroutine(enScript.Mort());
            }
            else
            {
                Moviment();
            }
        }
        else
        {
            _agent.isStopped = true;
        }

    }
    //move
    void FaceTarget()
    {
        // Vector3 direction = (player.position - transform.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //transform.rotation = lookRotation;
        transform.DOLookAt(player.position, 0.3f);



    }
    //move
    void Moviment()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (isAttackingPlayer == false)
        {
            if (distance <= lookDistance)
            {


                if (distance <= _agent.stoppingDistance)
                {


                    enScript.anEn.SetBool("IsInteracting", true);
                    isAttackingPlayer = true;
                    StartCoroutine(enAttack());








                }
                else
                {
                    enScript.anEn.SetBool("run", true);
                    _agent.SetDestination(player.position);

                }
            }
            else
            {
                enScript.anEn.SetBool("run", false);
                _agent.SetDestination(transform.position);
            }
        }


    }
    private IEnumerator enAttack()
    {


        if (!enScript.death)
        {
            FaceTarget();

            enScript.anEn.SetBool("run", false);
            _agent.isStopped = true;
            enScript.anEn.applyRootMotion = true;
            isAttackingP2 = true;
            enScript.anEn.Play("C_Combat_IDle");
            yield return new WaitForSeconds(0.35f);
        }


        if (!enScript.death)
        {

            enScript.anEn.Play("C_Jump_Forward");
            yield return new WaitForSeconds(1.1f);



            isAttackingP2 = false;
            hasHit = false;
            yield return new WaitForSeconds(0.8f);
            isAttackingPlayer = false;
            _agent.isStopped = false;
        }


    }

    private IEnumerator SoundWolf()
    {

        yield return new WaitForSeconds(Random.Range(wtAudio, wtAudio + randwtAudio));
        if(!enScript.death)
        {
            int r = Random.Range(0, barksAndGrowls.Length);
            enScript.typicalSound.clip = barksAndGrowls[r];
            enScript.typicalSound.Play();
            StartCoroutine(SoundWolf());
        }
    }
}
