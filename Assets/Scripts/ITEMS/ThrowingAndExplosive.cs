using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ThrowingAndExplosive : MonoBehaviour
{


    [SerializeField] private bool m_IsThrown;
    [Header("For Explosions")]
    [SerializeField] private float m_Radius;
    [SerializeField] private float m_DamagePlayer;
    [SerializeField] private float m_DamageEnemy;
    [SerializeField] private bool m_HitAirIgnore = false;
    [SerializeField] private LayerMask m_Lm;
    [SerializeField] private float m_IntensityShake = 7f;
    public void ThrowObject()
    {

    }
    
    
   
    private void OnTriggerEnter(Collider other)
    {
        if(m_HitAirIgnore == true && other.tag == "Air")
        {
            return;
        }
        if (((1 << other.gameObject.layer) & m_Lm) != 0)
        {
            return;
        }
        if(m_DamagePlayer == 0   && other.GetComponent<PlayerMain>() == null)
        {
            return;
        }
        if (other.tag != "floor" && other.tag != "weapon" && m_IsThrown == true && other.tag != "RoomCollider")
        {
            m_IsThrown = false;
            Vector3 pos = transform.position;
            transform.DOKill(transform);
            transform.position = pos;
            transform.GetComponent<Animator>().Play("Breakit");

        }
    }
    public void Explosion()
    {
        Collider[] info = Physics.OverlapSphere(transform.position, m_Radius);
        StartCoroutine(GameManager.gameManager._ProcessShake(m_IntensityShake, 6));
        foreach(Collider i in info)
        {
            if(i.gameObject == gameObject)
            {
                continue;
            }
            Enemy en = i.GetComponent<Enemy>();
            if(en!= null && i.tag != "Interactable" && i.tag != "Trap")
            {
                
   
                en.MalEn(m_DamageEnemy,transform.position);
            }
            if(i.GetComponent<PlayerMain>()!= null)
            {

                if(!Physics.Raycast(i.transform.position, (transform.position - i.transform.position), 2f, LayerMask.GetMask("Shield")))
                {
   
                    GameManager.gameManager.jugadorAtacat(m_DamagePlayer);
                }
            }
        }
    }

    public void DestroyAfterExplode()
    {
        Destroy(gameObject);
    }

    public bool isThrown { get => m_IsThrown; set => m_IsThrown = value; }
    public float radius => m_Radius;
    public float damagePlayer => m_DamagePlayer;
    public float damageEnemy => m_DamageEnemy;
    public bool hitAirIgnore => m_HitAirIgnore;
    public LayerMask lm => m_Lm;
    public float intensityShake => m_IntensityShake;

}
