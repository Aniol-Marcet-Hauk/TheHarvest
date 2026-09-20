using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossroom : MonoBehaviour
{
    public List<GameObject> activate;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            for (int i = 0; i < activate.Count; i++)
            {
                activate[i].SetActive(true);
            }
        }
    }
}
