using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyanimation : MonoBehaviour
{
    private Animator anEn;
    private Rigidbody rb;
    
    private void Start()
    {
        anEn = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
    }
    private void OnAnimatorMove()
    {
        if (!anEn.GetBool("IsInteracting"))
        {
            rb.velocity =Vector3.zero;
            return;
        }
        Vector3 pos = anEn.deltaPosition;

       
        rb.velocity = pos / Time.deltaTime;
    }
   
}
