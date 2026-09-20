using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    private bool m_HasChecked = false;
    [SerializeField] private Collider m_Coll;
    [SerializeField] private LayerMask m_Lm;
 
    private void Start()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMain>() == null && other.tag != "Bullet" && other.tag != "RoomCollider" && other.tag != "Player" && m_HasChecked == false)
        {
            if(((1 << other.gameObject.layer) & m_Lm) != 0)
            {
                return;
            }
            m_HasChecked = true;
            Stick(other);
            Destroy(m_Coll);
        }
    }
    private void Stick(Collider stickTo)
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
