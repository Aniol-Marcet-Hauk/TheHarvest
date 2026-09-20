using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ThrowingAndExplosive : MonoBehaviour
{


    public bool isThrown;
    [Header("For Explosions")]
    public float radius;
    public float damagePlayer,damageEnemy;
    public bool hitAirIgnore = false;
    public LayerMask lm;
    public float intensityShake = 7f;
    public void ThrowObject()
    {

    }
    
    
   
    private void OnTriggerEnter(Collider other)
    {
        if(hitAirIgnore == true && other.tag == "Air")
        {
            return;
        }
        if (((1 << other.gameObject.layer) & lm) != 0)
        {
            return;
        }
        if(damagePlayer == 0   && other.GetComponent<PlayerMain>() == null)
        {
            return;
        }
        if (other.tag != "floor" && other.tag != "weapon" && isThrown ==true && other.tag != "RoomCollider")
        {
            isThrown = false;
            Vector3 pos = transform.position;
            transform.DOKill(transform);
            transform.position = pos;
            transform.GetComponent<Animator>().Play("Breakit");

        }
    }
    public void Explosion()
    {
        Collider[] info = Physics.OverlapSphere(transform.position, radius);
        StartCoroutine(GameManager.gameManager._ProcessShake(intensityShake, 6));
        foreach(Collider i in info)
        {
            if(i.gameObject == gameObject)
            {
                continue;
            }
            Enemy en = i.GetComponent<Enemy>();
            if(en!= null && i.tag != "Interactable" && i.tag != "Trap")
            {
                
   
                en.MalEn(damageEnemy,transform.position);
            }
            if(i.GetComponent<PlayerMain>()!= null)
            {

                if(!Physics.Raycast(i.transform.position, (transform.position - i.transform.position), 2f, LayerMask.GetMask("Shield")))
                {
   
                    GameManager.gameManager.jugadorAtacat(damagePlayer);
                }
            }
        }
    }

    public void DestroyAfterExplode()
    {
        Destroy(gameObject);
    }

}
