using System;
using System.Collections;
using DigitalRuby.LightningBolt;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData; //SO

    [SerializeField]
    private Transform attackPoint;

    private SkillTreeManager skillTreeManager;

    private CooldownBar cooldownBar;

    private MasterAnimator playerAnimator;

    private int chosenSpecialMove;
    public LayerMask enemyLayers;

    public bool canUseSpecial;

    private int currentAttackCount;

    private float attackRange;

    public bool triggerSpin;

    // private bool isAttacking;

    private void Awake()
    {
        playerAnimator = GetComponent<MasterAnimator>();
        skillTreeManager = FindFirstObjectByType<SkillTreeManager>();
        triggerSpin = false;
    }

    private void Start()
    {
        attackRange = playerData.attackRange;
        canUseSpecial = false;
        // isAttacking = false;
        chosenSpecialMove = skillTreeManager.chosenSpecialMove;
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
        // if (context.started)
        // {
        // var animationID = Random.Range(6, 10);
        playerAnimator.ChangeAnimation(playerAnimator.swordAnimation[6]);
        // }
    }

    public void SpecialAttack(InputAction.CallbackContext context)
    {
        // mouse up

        if (canUseSpecial && context.started && chosenSpecialMove >= 0)
        {
            playerAnimator.ChangeAnimation(playerAnimator.specialAnimation[chosenSpecialMove]);
            CheckSpecialMove(chosenSpecialMove);
            Attack();
            cooldownBar.startDrain = true;
            currentAttackCount = 0;
        }
    }

    private void CheckSpecialMove(int chosenSpecialMove)
    {
        switch (chosenSpecialMove)
        {
            case 0:
                TriggerSpin();
                break;
            case 2:
                TriggerLightning();
                break;
            case 3:
                TriggerFog();
                break;
            case 1:
                TriggerSwordCombo();
                break;
        }
    }

    private void TriggerSwordCombo()
    {
        attackRange = playerData.attackRange + 5;
        StartCoroutine(ToggleSwordCombo(playerData.specialAttackCount));
    }

    private void TriggerFog()
    {
        FogController collisionFog = FindAnyObjectByType<FogController>();
        collisionFog.isPlaying = true;
        StartCoroutine(ToggleFog(playerData.specialAttackCount, collisionFog));
    }

    private void TriggerSpin()
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        gameManager.noDamage = true;
        StartCoroutine(ToggleSpin(playerData.specialAttackCount, gameManager));
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

    IEnumerator ToggleSpin(int count, GameManager gameManager)
    {
        yield return new WaitForSeconds(count);
        gameManager.noDamage = true;
    }

    IEnumerator ToggleSwordCombo(int count)
    {
        yield return new WaitForSeconds(count);
        attackRange = playerData.attackRange;
    }

    IEnumerator ToggleFog(int count, FogController collisionFog)
    {
        yield return new WaitForSeconds(count);
        collisionFog.isPlaying = false;
    }

    public void Attack()
    {
        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
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
