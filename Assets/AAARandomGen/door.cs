using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door: MonoBehaviour
{

    //maybe?
    public Room thisRoom;
    public Room connectedRoom;
    
    public GameObject doorObj,doorPrefab,minimapDoor;
    public bool special = false;
    private void Start()
    {
        if(special == false)
        {
            doorObj.SetActive(true);
        }
       
        
    }
    
    private void OnTriggerEnter(Collider other)
    {

        if (connectedRoom != null)
        {
            return;
        }
        StartCoroutine(CheckIfConnect(other));
        


        
    }
    IEnumerator CheckIfConnect(Collider other)
    {
        yield return new WaitForSeconds(.2f);
       
        float random = Random.Range(0f, 100f);

        if (other != null && connectedRoom == null && other.tag == "Door" && random < 80)
        {
            door door1 = other.GetComponent<door>();
            door1.connectedRoom = thisRoom;
            connectedRoom = door1.thisRoom;
        }
    }
    public void SetThisRoom(Room r)
    {
        thisRoom = r;
    }
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (connectedRoom != null)
        {
            return;
        }
        float random = Random.Range(0f, 100f);
        if (collision.GetComponent<door>() != null && random > 35)
        {
            connectedRoom = collision.GetComponentInParent<Room>();
        }
    }*/
    
}

