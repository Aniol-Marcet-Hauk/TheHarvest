using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThatMovesRandomly : MonoBehaviour
{

    public float speed;
    public float damage;
    private Rigidbody rbEn;
    public LayerMask Wall;
    private Enemy enScript;
    private void Start()
    {
        rbEn = transform.GetComponent<Rigidbody>();
        enScript = transform.GetComponent<Enemy>();
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + Random.Range(0, 360), transform.eulerAngles.z);
        enScript.typicalSound.pitch += Random.Range(-0.02f, 0.02f);
    }
    
    private void OnTriggerStay(Collider other)
    {
        
        if (Physics.CheckSphere(transform.position+transform.forward*0.5f, 0.5f, Wall))
        {


            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + Random.Range(100, 260), transform.eulerAngles.z);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.gameManager.jugadorAtacat(damage);

        }
    }

    private void Update()
    {
        
        if (!enScript.death)
        {
            if (enScript.enemyLife._vida == 0)
            {
                enScript.death = true;
                rbEn.velocity = Vector3.zero;
                StartCoroutine(enScript.Mort());
            }
            else
            {
                rbEn.velocity = transform.forward * speed;
            }

        }

    }

}
