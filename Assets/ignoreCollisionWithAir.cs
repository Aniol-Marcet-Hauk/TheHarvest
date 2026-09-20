using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignoreCollisionWithAir : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "Air"|| other.tag == "chest")
        {
            Physics.IgnoreCollision(other, gameObject.GetComponent<Collider>());
        }
    }
}
