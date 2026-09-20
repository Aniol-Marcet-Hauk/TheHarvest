using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDamage : MonoBehaviour
{
    [SerializeField] private TrapDamage m_Trapd;
    [SerializeField] private float m_Wait;
    void Start()
    {
        m_Trapd.enabled = false;
    }

    private IEnumerator TimeWait()
    {
        yield return new WaitForSeconds(m_Wait);
        m_Trapd.enabled = true;
    }
}
