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

    private CooldownBar cooldownBar;

    private MasterAnimator playerAnimator;

    private int specialAnimation;
    public LayerMask enemyLayers;

    private float specialAttackTime;

    public bool canUseSpecial;

    // private bool isAttacking;

    private void Awake()
    {
        playerAnimator = GetComponent<MasterAnimator>();
        skillTreeManager = FindFirstObjectByType<SkillTreeManager>();
    }

    private void Start()
    {
        specialAttackTime = playerData.specialAttackTime;
        canUseSpecial = false;
        // isAttacking = false;
        specialAnimation = skillTreeManager.chosenSpecialMove;
        cooldownBar = FindFirstObjectByType<CooldownBar>();
    }

    private void Update()
    {
        // Limits rate of attack, prevent spamming attack




        if (!canUseSpecial)
        {
            Debug.Log("can use special: " + canUseSpecial);
            cooldownBar.DrainCooldown();
        }
    }

    public void NormalAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var animationID = Random.Range(6, 10);
            playerAnimator.ChangeAnimation(playerAnimator.swordAnimation[6]);
        }
    }

    public void SpecialAttack(InputAction.CallbackContext context)
    {
        // mouse up
        // if (canUseSpecial && context.started)
        if (canUseSpecial && context.canceled)
        {
            Debug.Log("special: " + specialAnimation);
            playerAnimator.ChangeAnimation(playerAnimator.specialAnimation[specialAnimation]);
            Attack();
            canUseSpecial = false;
        }
    }

    public void Attack()
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
}
