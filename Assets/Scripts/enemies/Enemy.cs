using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class Enemy : MonoBehaviour
{



    [SerializeField] private float m_Health;
    [SerializeField] private HealthClass m_EnemyLife;
    [SerializeField] private Animator m_AnEn;
    [SerializeField] private bool m_Death;
    //probably don't need a bool knockBackFieldJustHave a knockBack speed and if its
    [SerializeField] private bool m_KnockBack = false;
    [Space]

    [SerializeField] private HealthBar m_HealthBarEnemy;
    [SerializeField] private float m_SpeedKnockBack;
    [SerializeField] private float m_DistanceKnockBack = 10f;
    [SerializeField] private LayerMask m_KnockBackLayers;
    [SerializeField] private bool m_HasKnockBackAn = false;
    [SerializeField] private AudioSource m_DamageSound, m_TypicalSound;

    public float Health { get { return m_Health; } set { m_Health = value; } }
    public HealthClass EnemyLife { get { return m_EnemyLife; } set { m_EnemyLife = value; } }
    public Animator AnEn { get { return m_AnEn; } set { m_AnEn = value; } }
    public bool Death { get { return m_Death; } set { m_Death = value; } }
    public bool KnockBack { get { return m_KnockBack; } set { m_KnockBack = value; } }
    public HealthBar HealthBarEnemy { get { return m_HealthBarEnemy; } set { m_HealthBarEnemy = value; } }
    public float SpeedKnockBack { get { return m_SpeedKnockBack; } set { m_SpeedKnockBack = value; } }
    public float DistanceKnockBack { get { return m_DistanceKnockBack; } set { m_DistanceKnockBack = value; } }
    public LayerMask KnockBackLayers { get { return m_KnockBackLayers; } set { m_KnockBackLayers = value; } }
    public AudioSource DamageSound { get { return m_DamageSound; } set { m_DamageSound = value; } }
    public AudioSource TypicalSound { get { return m_TypicalSound; } set { m_TypicalSound = value; } }
    
    void Start()
    {
        //_agent = GetComponent<NavMeshAgent>();
        m_EnemyLife = new HealthClass(m_Health, m_Health);
        m_AnEn = GetComponentInChildren<Animator>();
        if(m_HealthBarEnemy != null)
        {
            m_HealthBarEnemy.SetMaxHealth(m_EnemyLife._vidaMax);
            m_HealthBarEnemy.SetHealth(m_EnemyLife._vida);
        }
    }

    //this is what actually stays inside enemy
    public IEnumerator Mort()
    {
        //de moment només faig que desaparegui

        transform.GetComponent<Collider>().enabled = false;
        GameManager.gameManager.RAGE += 8f;
        GameManager.gameManager.coins += Random.Range(0, 2) * 5;
        m_AnEn.SetBool("run", false);
        m_AnEn.Play("Death");
        yield return new WaitForSeconds(2.3f);
        float randomItemChance = Random.Range(0, 100);
        if(randomItemChance < 7.5f)
        {
            GameObject itemSpawn = GameManager.gameManager.chooseRandItem(GameManager.gameManager.allItems);
            Instantiate(itemSpawn, transform.position, transform.rotation);
        }

        Destroy(gameObject);
        Destroy(gameObject);
    }

    public void MalEn(float malFet, Vector3 posicioMal)
    {
        if(m_AnEn.GetBool("Stun") == false && !m_Death)
        {
            m_AnEn.SetBool("Stun", true);
            m_EnemyLife.CanviarVida(-malFet);
            if(m_HealthBarEnemy != null)
            {
                m_HealthBarEnemy.SetHealth(m_EnemyLife._vida);
            }
            if(m_KnockBack == true)//eliminate this bool its stupid
            {
                TakeKnockBack(posicioMal, m_DistanceKnockBack, m_SpeedKnockBack);
            }
            m_AnEn.Play("Damaged");
            if(m_DamageSound != null && posicioMal != Vector3.zero)
            {
                m_DamageSound.Play();
            }
            StartCoroutine("StunTrue");
        }
    }
    public void TakeKnockBack(Vector3 knockbackPos, float distanceKnockBack, float speedKnockBack)//add distance to kncokbackThrough here and speed of knockbackHere
    {
        if(knockbackPos == Vector3.zero)
        {
            return;
        }

        Vector3 endPos = (knockbackPos - transform.position).normalized * distanceKnockBack * -1f;
        endPos.y = 0f;
        RaycastHit ray;
        if (Physics.Raycast(transform.position, endPos - transform.position, out ray, distanceKnockBack, m_KnockBackLayers))
        {
            transform.DOMove(transform.position + endPos * ray.distance / m_DistanceKnockBack, speedKnockBack * ray.distance / m_DistanceKnockBack).OnComplete(() => m_AnEn.Play("finishKnockBack"));
        }
        if(m_HasKnockBackAn == true)
        {
            transform.DOMove(transform.position + endPos, speedKnockBack).OnComplete(() => m_AnEn.Play("finishKnockBack"));
        }
        else
        {
            transform.DOMove(transform.position + endPos, speedKnockBack);
        }
    }
    private IEnumerator StunTrue()
    {
        yield return new WaitForSeconds(0.1f);
        m_AnEn.SetBool("Stun", false);
    }
    public bool GetStun()
    {
        return m_AnEn.GetBool("Stun");
    }
    public IEnumerator ConstantDamage(float strength, int duration, int iteration)
    {
        m_EnemyLife.CanviarVida(-strength);
        if(m_HealthBarEnemy != null)
        {
            m_HealthBarEnemy.SetHealth(m_EnemyLife._vida);
        }

        yield return new WaitForSeconds(1f);
        iteration++;
        if (iteration < duration)
        {
            StartCoroutine(ConstantDamage(strength, duration, iteration));
        }
    }
    
}
