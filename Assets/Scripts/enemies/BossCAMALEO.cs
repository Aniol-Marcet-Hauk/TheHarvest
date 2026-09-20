using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossCAMALEO : MonoBehaviour
{
    private Enemy m_EnScript;
    private NavMeshAgent m_Agent;

    private void Start()
    {
        m_EnScript = transform.GetComponent<Enemy>();
        m_Agent = transform.GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (!m_EnScript.Death)//add a bool to enemy script that checks for a Stun2???
        {
            if (m_EnScript.EnemyLife._vida == 0f)
            {
                m_EnScript.Death = true;
                StartCoroutine(m_EnScript.Mort());
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
