using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDamage : MonoBehaviour
{
    public TrapDamage trapd;
    public float wait;
    void Start()
    {
        trapd.enabled = false;
    }

    IEnumerator TimeWait()
    {
        yield return new WaitForSeconds(wait);
        trapd.enabled = true;
    }
}
