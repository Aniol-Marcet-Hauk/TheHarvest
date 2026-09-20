using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterDamagePlayerAndEnemy : MonoBehaviour
{
    [SerializeField] private bool damagePlayer, damageEnemy;
    [SerializeField] private float damageAmountPlayer, damageAmountEnemy;
    [SerializeField] private Collider colliderForEnableDamage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == gameObject)
        {
            return;
        }
        Enemy en = other.GetComponent<Enemy>();
        if (other.tag == "Player" && damagePlayer == true)
        {
            GameManager.gameManager.jugadorAtacat(damageAmountPlayer);

        }
        else if(en != null && damageEnemy == true)
        {
            en.MalEn(damageAmountEnemy,transform.position);

        }
    }
    public void DestroyBULLET()
    {
       
        Destroy(gameObject);
    }
    public void EnableDamage()
    {
        colliderForEnableDamage.enabled = true;
    }
    public void DisableDamage()
    {
        colliderForEnableDamage.enabled = false;
    }
}
