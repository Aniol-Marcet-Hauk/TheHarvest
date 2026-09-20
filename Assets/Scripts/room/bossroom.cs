using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossroom : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_Activate;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            for (int i = 0; i < m_Activate.Count; i++)
            {
                m_Activate[i].SetActive(true);
            }
        }
    }
}
