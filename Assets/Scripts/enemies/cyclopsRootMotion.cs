using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cyclopsRootMotion : MonoBehaviour
{
    private Animator m_AnEn;
    private Rigidbody m_Rb;

    private void Start()
    {
        m_AnEn = GetComponent<Animator>();
        m_Rb = GetComponentInParent<Rigidbody>();
    }
    private void OnAnimatorMove()
    {

        Vector3 pos = m_AnEn.deltaPosition;
        Vector3 rot = m_AnEn.deltaRotation.eulerAngles;
        Debug.Log(rot);
        rot.z = 0;
        rot.x = 0;
        m_Rb.velocity = pos / Time.deltaTime;
        m_Rb.angularVelocity = rot / Time.deltaTime;
    }

}
