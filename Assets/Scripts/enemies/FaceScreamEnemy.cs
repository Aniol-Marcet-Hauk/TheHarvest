using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FaceScreamEnemy : MonoBehaviour
{
    public float damage;
    public float speed,reducedSpeedWhenHasHit;
    
    public float timeRotation = 0.1f;
    public float timeItTakesForAttackAgain;
    public float distanceToMoveTo;
    [SerializeField]
    private float lookDistance;
    [SerializeField]
    private Transform player;
    private Enemy enScript;
    private bool hasHitPlayer;
    private NavMeshAgent _agent;
    private Vector3 rotateTo;
    
    private void Start()
    {
        enScript = transform.GetComponent<Enemy>();
        _agent = transform.GetComponent<NavMeshAgent>();
        player = GameManager.gameManager.PLAYER.transform;
        enScript.TypicalSound.enabled = false;
        StartCoroutine(soundON());
    }
    private IEnumerator soundON()
    {
        yield return new WaitForSeconds(Random.Range(0f, 3f));
        enScript.TypicalSound.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" )
        {
            

            GameManager.gameManager.jugadorAtacat(damage);
            rotateTo = transform.position + transform.forward * distanceToMoveTo;
            hasHitPlayer = true;
            StartCoroutine(goBackForHit());
        }
    }

    private void Update()
    {
        if (!enScript.Death)
        {
            if(enScript.EnemyLife._vida == 0)
            {
                enScript.Death = true;
                _agent.isStopped = true;
                StartCoroutine(enScript.Mort());
                enScript.TypicalSound.mute = true;
            }
            else
            {
                float pitchRand = Random.Range(0, 1f);
                if (enScript.TypicalSound.pitch >= 1.5f)
                {
                    enScript.TypicalSound.pitch -= Time.deltaTime * pitchRand;
                }
                else if(enScript.TypicalSound.pitch <= .8f)
                {
                    enScript.TypicalSound.pitch += Time.deltaTime * pitchRand;
                }
                else
                {
                    int randSign = Random.Range(-1, 1)*2;
                    enScript.TypicalSound.pitch += Time.deltaTime * (pitchRand + (float)randSign * pitchRand);
                }
                    
                Moviment();
            }
            
        }
        else
        {
            
        }
    }
    private void Moviment()
    {
        /*float distance = Vector3.Distance(player.position, transform.position);
        if(distance <= lookDistance)
        {*/

        if (hasHitPlayer == false)
        {
            //Rotate();
            player = GameManager.gameManager.PLAYER.transform;
            _agent.speed = speed;
            _agent.SetDestination(player.position);
        }
        else
        {
            _agent.speed = reducedSpeedWhenHasHit;

            _agent.SetDestination(rotateTo);
        }

        /*}
        else
        {
            _agent.isStopped = true;
        }*/

    }
    private IEnumerator goBackForHit()
    {
        yield return new WaitForSeconds(timeItTakesForAttackAgain);
        hasHitPlayer = false;
    }
}
