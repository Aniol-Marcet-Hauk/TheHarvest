using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FleeFromPlayer : MonoBehaviour
{
    private NavMeshAgent thisAgent;
    private Transform player;
    public Enemy enScript;
    [SerializeField] private float distanceStartRun;
    private Animator an;
    [SerializeField] private GameObject audioS;
    void Start()
    {
        thisAgent = transform.GetComponent<NavMeshAgent>();
        player = GameManager.gameManager.PLAYER.transform;
        an = GetComponent<Animator>();

    }

    
    void Update()
    {
        if (enScript != null && !enScript.Death)
        {
            if (enScript.EnemyLife._vida == 0f)
            {
                thisAgent.isStopped=true;
                enScript.Death = true;
                thisAgent.isStopped = true;
                StartCoroutine(enScript.Mort());
            }
            else
            {
                float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
                
                if (distanceFromPlayer < distanceStartRun)
                {
                    an.SetBool("run", true);
                    audioS.SetActive(true);
                    Vector3 playerDirection = transform.position - player.position;
                    thisAgent.SetDestination(transform.position + playerDirection);
                    return;
                }
                audioS.SetActive(false);
                an.SetBool("run", false);
            }
        }
        else
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceFromPlayer < distanceStartRun)
            {
                an.SetBool("run", true);
                audioS.SetActive(true);
                Vector3 playerDirection = transform.position - player.position;
                thisAgent.SetDestination(transform.position + playerDirection);
                return;
            }
            audioS.SetActive(false);
            an.SetBool("run", false);
        }
        
    }
}
