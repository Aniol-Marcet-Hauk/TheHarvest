using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsSlowBullet : MonoBehaviour
{

    private PlayerMain pm;
    public Rigidbody Bullets;
    
    [SerializeField]private float distanceFromPlayer = 1.2f;
    [SerializeField] private float amountOfBullets, randomAngle, randomHeightAngle, speed,timeBeforeDestroy,coolDown;

    void Start()
    {

        GameManager.lightAttack += Gun;

        pm = GameManager.gameManager.PLAYER;

    }
    void Gun()
    {
        Vector3 pos = pm.transform.position + pm.transform.forward * distanceFromPlayer;
        for (int i = 0; i < amountOfBullets; i++)
        {
            Rigidbody r = Instantiate(Bullets, pos, pm.transform.rotation);
            r.gameObject.SetActive(true);
            float rand = Random.Range(-randomAngle * .5f, randomAngle * .5f);
            float randUp = Random.Range(-randomHeightAngle * .5f, randomHeightAngle * .5f);
            Quaternion randomizedDirection = Quaternion.Euler(0f, rand,randUp);
            Vector3 bulletDirection = randomizedDirection *pm.transform.forward;
            r.velocity = speed * bulletDirection;
            StartCoroutine(destroyBullet(r.gameObject));
            
        }
        pm.SetBoolCoolDown(true);
        StartCoroutine(coolDownOff());
    }
    IEnumerator destroyBullet(GameObject r)
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(r);
    }
    IEnumerator coolDownOff()
    {
        yield return new WaitForSeconds(coolDown);
        pm.SetBoolCoolDown(false);
    }
    private void OnDestroy()
    {
        pm.SetBoolCoolDown(false);
        GameManager.lightAttack -= Gun;
    }
}
