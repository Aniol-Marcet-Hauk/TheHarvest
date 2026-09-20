using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAnimation : MonoBehaviour
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
        if (!m_AnEn.GetBool("IsInteracting"))
        {
            m_Rb.velocity = Vector3.zero;
            return;
        }
        Vector3 deltaPos = m_AnEn.deltaPosition;
        m_Rb.velocity = deltaPos / Time.deltaTime;
    }
}
