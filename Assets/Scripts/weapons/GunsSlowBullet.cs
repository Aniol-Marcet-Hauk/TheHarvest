using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsSlowBullet : MonoBehaviour
{

    private PlayerMain m_Pm;
    [SerializeField] private Rigidbody m_Bullets;
    
    [SerializeField]private float distanceFromPlayer = 1.2f;
    [SerializeField] private float amountOfBullets, randomAngle, randomHeightAngle, speed,timeBeforeDestroy,coolDown;

    void Start()
    {

        GameManager.lightAttack += Gun;

        m_Pm = GameManager.gameManager.PLAYER;

    }
    private void Gun()
    {
        Vector3 pos = m_Pm.transform.position + m_Pm.transform.forward * distanceFromPlayer;
        for (int i = 0; i < amountOfBullets; i++)
        {
            Rigidbody r = Instantiate(m_Bullets, pos, m_Pm.transform.rotation);
            r.gameObject.SetActive(true);
            float rand = Random.Range(-randomAngle * .5f, randomAngle * .5f);
            float randUp = Random.Range(-randomHeightAngle * .5f, randomHeightAngle * .5f);
            Quaternion randomizedDirection = Quaternion.Euler(0f, rand,randUp);
            Vector3 bulletDirection = randomizedDirection * m_Pm.transform.forward;
            r.velocity = speed * bulletDirection;
            StartCoroutine(DestroyBullet(r.gameObject));
            
        }
        m_Pm.SetBoolCoolDown(true);
        StartCoroutine(CoolDownOff());
    }
    private IEnumerator DestroyBullet(GameObject r)
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(r);
    }
    private IEnumerator CoolDownOff()
    {
        yield return new WaitForSeconds(coolDown);
        m_Pm.SetBoolCoolDown(false);
    }
    private void OnDestroy()
    {
        m_Pm.SetBoolCoolDown(false);
        GameManager.lightAttack -= Gun;
    }
}
