using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class WolfEnemy : MonoBehaviour
{
    [SerializeField]
    private float m_LookDistance = 10f;
    [SerializeField]
    private Transform m_Player;
    [SerializeField]
    private float m_Damage;
    private bool m_HasHit = false;
    private bool m_IsAttackingPlayer = false, m_IsAttackingP2 = false;
    private Enemy m_EnScript;
    private NavMeshAgent m_Agent;
    [Header("sounds")]
    [SerializeField] private AudioClip[] m_BarksAndGrowls;
    [SerializeField] private float m_WtAudio, m_RandwtAudio;

    public float Damage { get { return m_Damage; } }
    public Transform PlayerTransform { get { return m_Player; } }
    void Start()
    {
        m_EnScript = transform.GetComponent<Enemy>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Player = GameManager.gameManager.PLAYER.transform;
        StartCoroutine(SoundWolf());
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && m_HasHit == false && m_IsAttackingPlayer && m_IsAttackingP2)
        {
            m_HasHit = true;
            GameManager.gameManager.jugadorAtacat(m_Damage);
        }
    }
    void Update()
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
        else
        {
            m_Agent.isStopped = true;
        }
    }
    //move
    void FaceTarget()
    {
        transform.DOLookAt(m_Player.position, 0.3f);
    }
    //move
    void Moviment()
    {
        float distance = Vector3.Distance(m_Player.position, transform.position);
        if (m_IsAttackingPlayer == false)
        {
            if (distance <= m_LookDistance)
            {
                if (distance <= m_Agent.stoppingDistance)
                {
                    m_EnScript.AnEn.SetBool("IsInteracting", true);
                    m_IsAttackingPlayer = true;
                    StartCoroutine(EnAttack());
                }
                else
                {
                    m_EnScript.AnEn.SetBool("run", true);
                    m_Agent.SetDestination(m_Player.position);
                }
            }
            else
            {
                m_EnScript.AnEn.SetBool("run", false);
                m_Agent.SetDestination(transform.position);
            }
        }
    }
    private IEnumerator EnAttack()
    {
        if (!m_EnScript.Death)
        {
            FaceTarget();
            m_EnScript.AnEn.SetBool("run", false);
            m_Agent.isStopped = true;
            m_EnScript.AnEn.applyRootMotion = true;
            m_IsAttackingP2 = true;
            m_EnScript.AnEn.Play("C_Combat_IDle");
            yield return new WaitForSeconds(0.35f);
        }

        if (!m_EnScript.Death)
        {
            m_EnScript.AnEn.Play("C_Jump_Forward");
            yield return new WaitForSeconds(1.1f);
            m_IsAttackingP2 = false;
            m_HasHit = false;
            yield return new WaitForSeconds(0.8f);
            m_IsAttackingPlayer = false;
            m_Agent.isStopped = false;
        }
    }

    private IEnumerator SoundWolf()
    {
        yield return new WaitForSeconds(Random.Range(m_WtAudio, m_WtAudio + m_RandwtAudio));
        if(!m_EnScript.Death)
        {
            int randomIndex = Random.Range(0, m_BarksAndGrowls.Length);
            m_EnScript.TypicalSound.clip = m_BarksAndGrowls[randomIndex];
            m_EnScript.TypicalSound.Play();
            StartCoroutine(SoundWolf());
        }
    }
}
