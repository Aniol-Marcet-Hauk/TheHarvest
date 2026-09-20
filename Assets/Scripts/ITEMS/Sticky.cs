using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    private bool hasChecked = false;
    public Collider coll;
    public LayerMask lm;
 
    private void Start()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMain>() == null && other.tag != "Bullet" && other.tag != "RoomCollider" && other.tag != "Player" &&hasChecked==false)
        {
            if(((1 << other.gameObject.layer) & lm) != 0)
            {
                return;
            }
            hasChecked = true;
            STICK(other);
            Destroy(coll);
        }
    }
    void STICK(Collider stickTo)
    {
        
       
        float x = transform.localScale.x / stickTo.transform.lossyScale.x;
        float y = transform.localScale.y / stickTo.transform.lossyScale.y;
        float z = transform.localScale.z / stickTo.transform.lossyScale.z;
        // transform.localScale = new Vector3(x, y ,z);
        transform.parent = stickTo.transform;
        transform.localScale = new Vector3(x, y, z);
        transform.localEulerAngles = Vector3.zero;
        
        transform.GetComponent<Rigidbody>().isKinematic = true;
    }
}
