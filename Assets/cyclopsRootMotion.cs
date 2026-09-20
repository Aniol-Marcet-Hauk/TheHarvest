using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cyclopsRootMotion : MonoBehaviour
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
        
        Vector3 pos = anEn.deltaPosition;
        Vector3 rot = anEn.deltaRotation.eulerAngles;
        Debug.Log(rot);
        rot.z = 0;
        rot.x = 0;
        rb.velocity = pos / Time.deltaTime;
        rb.angularVelocity = rot / Time.deltaTime;
    }

}
