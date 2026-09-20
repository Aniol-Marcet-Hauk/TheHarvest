using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawnItem : MonoBehaviour
{
    [SerializeField] private float m_TimeWait, m_TimeDestroyItem;
    private bool m_HasHit;
    [SerializeField] private bool m_DestroyOnceHit = false;//CreateOneThatDropsBombs
   
    [SerializeField] private Rigidbody m_SpawnItem;
    [SerializeField] private Transform m_SpawnPosRotation;
    [SerializeField] private float m_Speed;

    private void OnTriggerStay(Collider other)
    {
        if (m_HasHit == true)
        {
            return;
        }
        
        if (other.tag == "Player" || other.tag == "Enemy")
        {

            Rigidbody rb = Instantiate(m_SpawnItem, m_SpawnPosRotation.position, m_SpawnPosRotation.rotation);
            rb.gameObject.SetActive(true);
            if (m_Speed != 0)
            {
                rb.velocity = m_SpawnPosRotation.forward * m_Speed;
            }
            m_HasHit = true;
            StartCoroutine(TimeToNextHit());
            StartCoroutine(DestroyItem(rb.gameObject));
        }
        


    }

    private IEnumerator DestroyItem(GameObject des)
    {
        yield return new WaitForSeconds(m_TimeDestroyItem);
        Destroy(des);
    }
    private IEnumerator TimeToNextHit()
    {

        yield return new WaitForSeconds(m_TimeWait);
        if (m_DestroyOnceHit == true)
        {
            Destroy(gameObject);
        }
        m_HasHit = false;
    }

    public float timeWait { get => m_TimeWait; set => m_TimeWait = value; }
    public float timeDestroyItem { get => m_TimeDestroyItem; set => m_TimeDestroyItem = value; }
    public bool destroyOnceHit { get => m_DestroyOnceHit; set => m_DestroyOnceHit = value; }
}
