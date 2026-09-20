using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class Enemy : MonoBehaviour
{



    public float health;
    public HealthClass enemyLife;
    public Animator anEn;
    public bool death;
    //probably don't need a bool knockBackFieldJustHave a knockBack speed and if its
    public bool knockBack = false;
    [Space]

    public HealthBar healthBarEnemy;
    public float speedKnockBack;
    public float DistanceKnockBack = 10f;
    public LayerMask knockBackLayers;
    [SerializeField] private bool hasKnockBackAn = false;
    public AudioSource damageSound,typicalSound;
    
    void Start()
    {

        //_agent = GetComponent<NavMeshAgent>();
        enemyLife = new HealthClass(health, health);
        anEn = GetComponentInChildren<Animator>();
        if(healthBarEnemy != null)
        {
            healthBarEnemy.SetMaxHealth(enemyLife._vidaMax);
            healthBarEnemy.SetHealth(enemyLife._vida);
        }
        
        
    }

    
    //this is what actually stays inside enemy
    public IEnumerator Mort()
    {
        //de moment només faig que desaparegui

        transform.GetComponent<Collider>().enabled = false;
        GameManager.gameManager.RAGE += 8f;
        GameManager.gameManager.coins += Random.Range(0, 2) * 5;
        anEn.SetBool("run", false);
        anEn.Play("Death");
        yield return new WaitForSeconds(2.3f);
        float it = Random.Range(0, 100);
        if(it< 7.5f)
        {
            GameObject itspawn = GameManager.gameManager.chooseRandItem(GameManager.gameManager.allItems);
            Instantiate(itspawn,transform.position,transform.rotation);

        }

        
        Destroy(gameObject);
    }

    public void MalEn(float malFet,Vector3 posicioMal)
    {
        if(anEn.GetBool("Stun") == false && !death)
        {
            anEn.SetBool("Stun", true);
            enemyLife.CanviarVida(-malFet);
            if(healthBarEnemy != null)
            {
                healthBarEnemy.SetHealth(enemyLife._vida);
            }
            if(knockBack == true)//eliminate this bool its stupid
            {
                TakeKnockBack(posicioMal,DistanceKnockBack,speedKnockBack);
            }
            anEn.Play("Damaged");
            if(damageSound != null && posicioMal != Vector3.zero)
            {
                damageSound.Play();
            }
            StartCoroutine("StunTrue");
        }
       
    }
    public void TakeKnockBack(Vector3 _pos,float _distanceKnockBack,float _speedKnockBack)//add distance to kncokbackThrough here and speed of knockbackHere
    {
        if(_pos == Vector3.zero)
        {
            return;
           
        }

        Vector3 endPos = (_pos -transform.position ).normalized * _distanceKnockBack * -1f;
        endPos.y = 0f;
        RaycastHit ray;
        if (Physics.Raycast(transform.position, endPos - transform.position, out ray,_distanceKnockBack, knockBackLayers))
        {
            transform.DOMove(transform.position + endPos*ray.distance/DistanceKnockBack, _speedKnockBack * ray.distance / DistanceKnockBack).OnComplete(() => anEn.Play("finishKnockBack"));
        }
        if(hasKnockBackAn == true)
        {
            transform.DOMove(transform.position + endPos, _speedKnockBack).OnComplete(() => anEn.Play("finishKnockBack"));
        }
        else
        {
            
            transform.DOMove(transform.position + endPos, _speedKnockBack);
        }

    }
    private IEnumerator StunTrue()
    {
        yield return new WaitForSeconds(0.1f);
        anEn.SetBool("Stun", false);
    }
    public bool GetSTUN()
    {
        return anEn.GetBool("Stun");
    }
    public IEnumerator constantDamage(float _strength, int _duration, int i)
    {
        enemyLife.CanviarVida(-_strength);
        if(healthBarEnemy != null)
        {
            healthBarEnemy.SetHealth(enemyLife._vida);
        }
        
        yield return new WaitForSeconds(1f);
        i++;
        if (i < _duration)
        {
            StartCoroutine(constantDamage(_strength, _duration, i));
        }
    }
    
}
