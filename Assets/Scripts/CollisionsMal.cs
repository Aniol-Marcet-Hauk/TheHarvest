using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsMal : MonoBehaviour
{
    
    [SerializeField]
    private Collider coll;
    

    private void Awake()
    {
        
        coll.enabled = false;
        
        
    }
 
   
    public void TurnOnCollider()
    {
        coll.enabled = true;
        if(GameManager.gameManager.explodeWhenMelee == true)
        {
            Animator b = Instantiate(GameManager.gameManager.Bomb, coll.transform.position,Quaternion.identity);
            b.Play("Explosion");
        }
        
    }
    public void TurnOffCollider()
    {
        coll.enabled = false;
    }

}
