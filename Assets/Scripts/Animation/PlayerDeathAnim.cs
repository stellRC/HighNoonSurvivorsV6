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
        FindFirstObjectByType<GameManager>().isGameOver = true;
        // animator.enabled = false;
    }
}
