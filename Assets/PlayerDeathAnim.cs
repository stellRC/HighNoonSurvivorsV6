using UnityEngine;

public class PlayerDeathAnim : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        FindObjectOfType<GameManager>().isGameOver = true;
        // animator.enabled = false;
    }
}
