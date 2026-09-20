using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDespresAn : StateMachineBehaviour
{
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsInteracting", false);
        animator.applyRootMotion = false;
    }


}
