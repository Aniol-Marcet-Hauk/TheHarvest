using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsMal : MonoBehaviour
{
    
    [SerializeField]
    private Collider m_Coll;
    

    private void Awake()
    {
        
        m_Coll.enabled = false;
        
        
    }
 
   
    public void TurnOnCollider()
    {
        m_Coll.enabled = true;
        if(GameManager.gameManager.explodeWhenMelee == true)
        {
            Animator b = Instantiate(GameManager.gameManager.Bomb, m_Coll.transform.position,Quaternion.identity);
            b.Play("Explosion");
        }
        
    }
    public void TurnOffCollider()
    {
        m_Coll.enabled = false;
    }

}
