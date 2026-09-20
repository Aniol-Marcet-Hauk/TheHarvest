using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterDamagePlayerAndEnemy : MonoBehaviour
{
    [SerializeField] private bool m_DamagePlayer, m_DamageEnemy;
    [SerializeField] private float m_DamageAmountPlayer, m_DamageAmountEnemy;
    [SerializeField] private Collider m_ColliderForEnableDamage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == gameObject)
        {
            return;
        }
        Enemy en = other.GetComponent<Enemy>();
        if (other.tag == "Player" && m_DamagePlayer == true)
        {
            GameManager.gameManager.jugadorAtacat(m_DamageAmountPlayer);

        }
        else if(en != null && m_DamageEnemy == true)
        {
            en.MalEn(m_DamageAmountEnemy,transform.position);

        }
    }
    public void DestroyBULLET()
    {
       
        Destroy(gameObject);
    }
    public void EnableDamage()
    {
        m_ColliderForEnableDamage.enabled = true;
    }
    public void DisableDamage()
    {
        m_ColliderForEnableDamage.enabled = false;
    }
}
