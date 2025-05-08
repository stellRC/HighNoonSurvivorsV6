using UnityEngine;

public class PlayerAttackAnim : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        animator.gameObject.GetComponentInParent<PlayerCombat>().Attack();
    }
}
