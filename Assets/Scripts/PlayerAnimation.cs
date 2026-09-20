using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator an { get; private set;}
    
    //hauria de separar el player Main i el inputHandler ja que estic ficant les 2 coses en el mateix codi i aixó pot crear errors
    private PlayerMain playerMain;
    public bool potGrirar;
    private bool permetreCombo;
    
    void Start()
    {
        an = gameObject.GetComponent<Animator>();

        playerMain = gameObject.GetComponentInParent<PlayerMain>();
        an.applyRootMotion = false;
    }

    void Update()
    {
        
    }
    

    public void PlayerTargetAn(string animationName, bool isInteracting )
    {
        an.applyRootMotion = isInteracting;
        an.SetBool("IsInteracting", isInteracting);
        an.Play(animationName);
        
    }
    
    public void isAttacking(bool _state)
    {
        an.SetBool("AtacBool", _state);
    }
   
    private void OnAnimatorMove()
    {




        if (!an.GetBool("IsInteracting"))
        {

            return;
        }

        Vector3 pos = an.deltaPosition;

        //Debug.Log(pos);
        
        playerMain.player.velocity= pos/Time.deltaTime;

       

        

    }


    //aixo segurament hauria d'anar a playermanager pero ara mateix aqui és més simple
    public void activarIframesRoll()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }
    public void apagarIframesRoll()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public void PermetreComboTrue()
    {
        permetreCombo = true;
    }
    public void PermetreComboFalse()
    {
        permetreCombo = false;
    }
    public bool GetPermetreCombo()
    {
        return permetreCombo;
    }

}
