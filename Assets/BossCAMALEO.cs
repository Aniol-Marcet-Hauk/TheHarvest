using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossCAMALEO : MonoBehaviour
{
    private Enemy enScript;
    private NavMeshAgent agent;
    
    private void Start()
    {
        enScript = transform.GetComponent<Enemy>();
        agent = transform.GetComponent<NavMeshAgent>();
    }
    private void Update()
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
        
        
    }
    void Moviment()
    {

    }

    void TongueAttack()
    {
        //throws out toungue to hit player
    }
    void BiteAttack()
    {
        
    }
    void Throw3BombsAttack()
    {

    }
    void VomitAcid()
    {
        //this can literally be a trap script that does low damage in short time intervals
    }



}
