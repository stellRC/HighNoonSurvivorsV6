using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData; //SO

    [SerializeField]
    private Transform attackPoint;

    private SkillTreeManager skillTreeManager;

    private MasterAnimator playerAnimator;

    private int specialAnimation;
    public LayerMask enemyLayers;
    private float nextAttackTime;
    private float specialAttackTime;
    private bool canAttack;
    private bool canUseSpecial;

    // private bool isAttacking;

    private void Awake()
    {
        playerAnimator = GetComponent<MasterAnimator>();
        skillTreeManager = FindFirstObjectByType<SkillTreeManager>();
    }

    private void Start()
    {
        nextAttackTime = 0f;
        specialAttackTime = playerData.specialAttackTime;
        canUseSpecial = false;
        // isAttacking = false;
        specialAnimation = skillTreeManager.chosenSpecialMove;
    }

    private void Update()
    {
        // Limits rate of attack, prevent spamming attack
        if (Time.time >= nextAttackTime)
        {
            canAttack = true;
            nextAttackTime = Time.time + 1.0f / playerData.attackRate;
        }

        if (!canUseSpecial)
        {
            StartCoroutine(SpecialCooldown(specialAttackTime));
        }
    }

    IEnumerator SpecialCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canUseSpecial = true;
    }

    public void NormalAttack(InputAction.CallbackContext context)
    {
        // if (canAttack && context.canceled)
        if (context.canceled)
        {
            // canAttack = false;
            //Play attack animation
            // isAttacking = true;
            playerAnimator.ChangeAnimation(playerAnimator.swordAnimation[4]);
            Attack();
        }
    }

    public void SpecialAttack(InputAction.CallbackContext context)
    {
        // mouse up
        if (canUseSpecial && context.canceled)
        {
            canUseSpecial = false;
            // isAttacking = true;
            playerAnimator.ChangeAnimation(playerAnimator.specialAnimation[specialAnimation]);
            Attack();
        }
    }

    private void Attack()
    {
        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            playerData.attackRange,
            enemyLayers
        );

        // Damage Enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().DoDamage(playerData.attackDamage);
            }
        }
    }

    // void OnDrawGizmosSelected()
    // {
    //     if (attackPoint == null)
    //         return;

    //     Gizmos.DrawWireSphere(attackPoint.position, playerData.attackRange);
    // }
}
