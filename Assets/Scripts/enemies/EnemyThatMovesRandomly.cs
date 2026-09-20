using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThatMovesRandomly : MonoBehaviour
{

    [SerializeField] private float m_Speed;
    [SerializeField] private float m_Damage;
    private Rigidbody m_RbEn;
    [SerializeField] private LayerMask m_Wall;
    private Enemy m_EnScript;
    private void Start()
    {
        m_RbEn = transform.GetComponent<Rigidbody>();
        m_EnScript = transform.GetComponent<Enemy>();
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + Random.Range(0, 360), transform.eulerAngles.z);
        m_EnScript.TypicalSound.pitch += Random.Range(-0.02f, 0.02f);
    }
    
    private void OnTriggerStay(Collider other)
    {

        if (Physics.CheckSphere(transform.position+transform.forward*0.5f, 0.5f, m_Wall))
        {


            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + Random.Range(100, 260), transform.eulerAngles.z);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.gameManager.jugadorAtacat(m_Damage);

        }
    }

    private void Update()
    {
        HandleMovementAndDeathState();
    }

    private void HandleMovementAndDeathState()
    {
        if (!m_EnScript.Death)
        {
            if (m_EnScript.EnemyLife._vida == 0)
            {
                m_EnScript.Death = true;
                m_RbEn.velocity = Vector3.zero;
                StartCoroutine(m_EnScript.Mort());
            }
            else
            {
                m_RbEn.velocity = transform.forward * m_Speed;
            }
        }
    }

}
