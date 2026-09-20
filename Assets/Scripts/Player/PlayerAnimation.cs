using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator Animator { get; private set; }
    
    //hauria de separar el player Main i el inputHandler ja que estic ficant les 2 coses en el mateix codi i aixó pot crear errors
    private PlayerMain playerMain;
    [SerializeField] private bool m_PotGirar;
    private bool permetreCombo;
    
    void Start()
    {
        Animator = gameObject.GetComponent<Animator>();

        playerMain = gameObject.GetComponentInParent<PlayerMain>();
        Animator.applyRootMotion = false;
    }

    void Update()
    {
        
    }
    

    public void PlayerTargetAn(string animationName, bool isInteracting )
    {
        Animator.applyRootMotion = isInteracting;
        Animator.SetBool("IsInteracting", isInteracting);
        Animator.Play(animationName);
        
    }
    
    public void SetIsAttacking(bool state)
    {
        Animator.SetBool("AtacBool", state);
    }
   
    private void OnAnimatorMove()
    {




        if (!Animator.GetBool("IsInteracting"))
        {

            return;
        }

        Vector3 pos = Animator.deltaPosition;

        //Debug.Log(pos);
        
        playerMain.player.velocity= pos/Time.deltaTime;

       

        

    }


    //aixo segurament hauria d'anar a playermanager pero ara mateix aqui és més simple
    public void EnableRollIframes()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }
    public void DisableRollIframes()
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

    public Animator an => Animator;
    public void isAttacking(bool state) => SetIsAttacking(state);
    public void activarIframesRoll() => EnableRollIframes();
    public void apagarIframesRoll() => DisableRollIframes();

}
