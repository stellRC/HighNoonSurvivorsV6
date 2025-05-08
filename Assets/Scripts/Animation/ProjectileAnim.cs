using UnityEngine;

public class ProjectileAnim : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Instantiate projectile when animation finishes
        // animator
        //     .gameObject.GetComponentInParent<ProjectileWeapon>()
        //     .InstantiateProjectile(animator.rootPosition);
        animator.gameObject.GetComponentInParent<ProjectileWeapon>().InstantiateProjectile();
    }
}
