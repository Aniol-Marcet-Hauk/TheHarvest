using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawnItem : MonoBehaviour
{
    public float  timeWait,timeDestroyItem;
    private bool hasHit;
    public bool destroyOnceHit = false;//CreateOneThatDropsBombs
   
    [SerializeField] private Rigidbody spawnItem;
    [SerializeField] private Transform spawnPosRotation;
    [SerializeField] private float speed;

    private void OnTriggerStay(Collider other)
    {
        if (hasHit == true)
        {
            return;
        }
        
        if (other.tag == "Player" || other.tag == "Enemy")
        {

            Rigidbody rb = Instantiate(spawnItem, spawnPosRotation.position, spawnPosRotation.rotation);
            rb.gameObject.SetActive(true);
            if (speed != 0)
            {
                rb.velocity = spawnPosRotation.forward * speed;
            }
            hasHit = true;
            StartCoroutine(TimeToNextHit());
            StartCoroutine(destroyItem(rb.gameObject));
        }
        


    }

    IEnumerator destroyItem(GameObject des)
    {
        yield return new WaitForSeconds(timeDestroyItem);
        Destroy(des);
    }
    private IEnumerator TimeToNextHit()
    {

        yield return new WaitForSeconds(timeWait);
        if (destroyOnceHit == true)
        {
            Destroy(gameObject);
        }
        hasHit = false;
    }
}
