using System.Collections;
using DigitalRuby.LightningBolt;
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

    public bool canUseSpecial;

    private int currentAttackCount;

    private float attackRange;

    // private bool isAttacking;

    private void Awake()
    {
        playerAnimator = GetComponent<MasterAnimator>();
        skillTreeManager = FindFirstObjectByType<SkillTreeManager>();
    }

    private void Start()
    {
        attackRange = playerData.attackRange;
        canUseSpecial = false;
        // isAttacking = false;
        specialAnimation = skillTreeManager.chosenSpecialMove;
        cooldownBar = FindFirstObjectByType<CooldownBar>();
        currentAttackCount = 0;
    }

    void Update()
    {
        // If number of killed enemies is == 10, special attack is true
        // Each enemy kill fills the bar
        // If special attack is used, drain bar and reset attack number needed
        if (currentAttackCount >= playerData.specialAttackCount)
        {
            canUseSpecial = true;
        }
        else
        {
            canUseSpecial = false;
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

        if (canUseSpecial && context.started)
        {
            playerAnimator.ChangeAnimation(playerAnimator.specialAnimation[specialAnimation]);
            TriggerLightning();
            Attack();
            cooldownBar.startDrain = true;
            currentAttackCount = 0;
        }
    }

    private void TriggerLightning()
    {
        LightningBoltScript lightningBoltScript = FindFirstObjectByType<LightningBoltScript>();
        lightningBoltScript.ManualMode = false;
        StartCoroutine(ToggleLightning(playerData.specialAttackCount, lightningBoltScript));
    }

    IEnumerator ToggleLightning(int count, LightningBoltScript lightningBoltScript)
    {
        yield return new WaitForSeconds(count);
        lightningBoltScript.ManualMode = true;
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
                currentAttackCount++;
                cooldownBar.RefillCooldown();
            }
        }
    }
}
