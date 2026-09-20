using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public float damage,damageEn,timeWait;
    private bool hasHit;
    public bool destroyOnceHit = false;
    private void OnTriggerStay(Collider other)
    {
        if(hasHit == true)
        {
            return;
        }
        Enemy en = other.GetComponent<Enemy>();
        if (other.tag == "Player" && damage != 0)
        {

            hasHit = true;
            GameManager.gameManager.jugadorAtacat(damage);
            StartCoroutine(TimeToNextHit());

        }
        else if(en!= null && damageEn != 0)
        {
            hasHit = true;
            en.MalEn(damageEn,Vector3.zero);
            StartCoroutine(TimeToNextHit());
        }


    }
    private IEnumerator TimeToNextHit()
    {

        yield return new WaitForSeconds(timeWait);
        if(destroyOnceHit == true)
        {
            Destroy(gameObject);
        }
        hasHit = false;
    }
}
