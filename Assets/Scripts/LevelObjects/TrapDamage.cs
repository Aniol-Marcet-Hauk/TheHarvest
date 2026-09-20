using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField] private float m_Damage, m_DamageEn, m_TimeWait;
    private bool m_HasHit;
    [SerializeField] private bool m_DestroyOnceHit = false;
    private void OnTriggerStay(Collider other)
    {
        if(m_HasHit == true)
        {
            return;
        }
        Enemy en = other.GetComponent<Enemy>();
        if (other.tag == "Player" && m_Damage != 0)
        {

            m_HasHit = true;
            GameManager.gameManager.jugadorAtacat(m_Damage);
            StartCoroutine(TimeToNextHit());

        }
        else if(en!= null && m_DamageEn != 0)
        {
            m_HasHit = true;
            en.MalEn(m_DamageEn,Vector3.zero);
            StartCoroutine(TimeToNextHit());
        }


    }
    private IEnumerator TimeToNextHit()
    {

        yield return new WaitForSeconds(m_TimeWait);
        if(m_DestroyOnceHit == true)
        {
            Destroy(gameObject);
        }
        m_HasHit = false;
    }

    public float damage { get => m_Damage; set => m_Damage = value; }
    public float damageEn { get => m_DamageEn; set => m_DamageEn = value; }
    public float timeWait { get => m_TimeWait; set => m_TimeWait = value; }
    public bool destroyOnceHit { get => m_DestroyOnceHit; set => m_DestroyOnceHit = value; }
}
