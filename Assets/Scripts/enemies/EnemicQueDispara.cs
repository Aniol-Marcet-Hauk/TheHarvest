using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
public class EnemicQueDispara : MonoBehaviour
{
    [SerializeField] private float m_DistanceBeforeStartShoots, m_DistanceOfShot, m_SpeedShot, m_TimeBeforeAttack, m_TimeRandBeforeAttack, m_SpeedRotates = 5f;
    [SerializeField] private GameObject m_Shot;
    [SerializeField] private float m_JumpHeight, m_DestroyAnimationDuration = 0.5f;
    [SerializeField] private int m_NumOfJumps = 0;
    [SerializeField] private Transform m_EndPosition;
    private Transform m_Player;
    private bool m_IsAttacking;
    private Enemy m_EnScript;
    private NavMeshAgent m_Nav;
    [SerializeField] private bool m_Shake = false;
    private void Start()
    {
        m_Player = GameManager.gameManager.PLAYER.transform;
        m_EnScript = transform.GetComponent<Enemy>();
        m_Nav = transform.GetComponent<NavMeshAgent>();
        if (m_Nav != null)
        {
            m_Nav.updateRotation = false;
        }

    }
    private void Update()
    {
        if (!m_EnScript.Death)
        {
            if (m_EnScript.EnemyLife._vida == 0f)
            {
                m_EnScript.Death = true;
                if(m_Nav != null)
                {
                    m_Nav.isStopped = true;
                }
                StartCoroutine(m_EnScript.Mort());
            }
            else
            {
                ComençaADisparar();
            }
        }
        
        
    }
    private void ComençaADisparar()
    {
        //Either Make It so it doesnt immediately rotate to look at the player or something like that
        
        


        Vector3 rotDirection = (m_Player.position - transform.position).normalized;
        Quaternion inputRotation = Quaternion.LookRotation(rotDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, inputRotation, m_SpeedRotates * Time.deltaTime);
        float distanceFromPlayer = Vector3.Distance(transform.position, m_Player.position);
        if (distanceFromPlayer < m_DistanceBeforeStartShoots && m_IsAttacking == false)
        {
            m_IsAttacking = true;
            StartCoroutine("Shoot");
        }
    }
    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(m_TimeBeforeAttack + Random.Range(-m_TimeRandBeforeAttack, m_TimeRandBeforeAttack));
        if (!m_EnScript.Death)
        {
            Transform activeShot = Instantiate(m_Shot, m_Shot.transform.position, m_Shot.transform.rotation).transform;
            activeShot.gameObject.SetActive(true);

            activeShot.DOJump(m_EndPosition.position, m_JumpHeight, m_NumOfJumps, m_SpeedShot);
            if(m_EnScript.TypicalSound != null)
            {
                m_EnScript.TypicalSound.Play();
            }
            if(m_Shake)
            {
                StartCoroutine(GameManager.gameManager._ProcessShake(3.5f, 2f, .015f));
            }
            m_IsAttacking = false;
            yield return new WaitForSeconds(m_SpeedShot + 0.1f);
            if (activeShot != null)
            {
                activeShot.GetComponent<Animator>().Play("Breakit");
                yield return new WaitForSeconds(m_DestroyAnimationDuration);
                Destroy(activeShot.gameObject);
            }

        }


    }
    
   

}
