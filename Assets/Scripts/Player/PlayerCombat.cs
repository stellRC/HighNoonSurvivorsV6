using System;
using System.Collections;
using DigitalRuby.LightningBolt;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData; //SO

    [SerializeField]
    private Transform attackPoint;

    private CinemachineImpulseSource impulseSource;

    private SkillTreeManager skillTreeManager;

    private CooldownBar cooldownBar;

    private MasterAnimator playerAnimator;

    private int chosenSpecialMove;
    public LayerMask enemyLayers;

    public bool canUseSpecial;

    private int currentAttackCount;

    private float attackRange;

    public bool triggerSpin;
    private float popEnemyTime;

    private bool isPopping;

    private void Awake()
    {
        playerAnimator = GetComponent<MasterAnimator>();
        skillTreeManager = FindFirstObjectByType<SkillTreeManager>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        triggerSpin = false;
    }

    private void Start()
    {
        attackRange = playerData.attackRange;
        canUseSpecial = false;

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

        if (isPopping)
        {
            popEnemyTime += Time.deltaTime;
            if (popEnemyTime >= 1)
            {
                PopEnemy();
                popEnemyTime = 0;
            }
        }
    }

    public void NormalAttack(InputAction.CallbackContext context)
    {
        if (context.started && !skillTreeManager.isSpecialAnim)
        {
            // var animationID = Random.Range(6, 10);
            // playerAnimator.ChangeAnimation(playerAnimator.swordAnimation[6]);
            playerAnimator.isAttacking = true;
            Attack();
        }
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

    // Player's attack range increases
    private void TriggerSwordCombo()
    {
        attackRange = playerData.attackRange + 5;
        CameraShake.instance.ScreenShakeFromProfile(3, impulseSource);
        StartCoroutine(ToggleSwordCombo(playerData.specialAttackCount));
    }

    // Player becomes impervious to damage
    private void TriggerSpin()
    {
        GameManager.Instance.noDamage = true;
        playerAnimator.noDamage = true;
        CameraShake.instance.ScreenShakeFromProfile(1, impulseSource);
        StartCoroutine(ToggleSpin(playerData.specialAttackCount));
    }

    // Destroy enemies positioned between player and clock
    private void TriggerLightning()
    {
        LightningBoltScript lightningBoltScript = FindFirstObjectByType<LightningBoltScript>();
        lightningBoltScript.ManualMode = false;
        playerAnimator.isShocking = true;
        CameraShake.instance.ScreenShakeFromProfile(0, impulseSource);
        StartCoroutine(ToggleLightning(playerData.specialAttackCount, lightningBoltScript));
    }

    // Randomly destroy enemies every second for length of special attack
    private void TriggerFog()
    {
        FogController collisionFog = FindAnyObjectByType<FogController>();
        collisionFog.isPlaying = true;
        isPopping = true;
        CameraShake.instance.ScreenShakeFromProfile(2, impulseSource);
        StartCoroutine(ToggleFog(playerData.specialAttackCount, collisionFog));
    }

    IEnumerator ToggleLightning(int count, LightningBoltScript lightningBoltScript)
    {
        yield return new WaitForSeconds(count);

        lightningBoltScript.ManualMode = true;
        playerAnimator.isShocking = false;
    }

    IEnumerator ToggleSpin(int count)
    {
        yield return new WaitForSeconds(count);

        GameManager.Instance.noDamage = false;
        playerAnimator.noDamage = false;
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
        isPopping = false;
    }

    private void PopEnemy()
    {
        // enemy pop time is one for every second in special attack count
        var enemy = FindFirstObjectByType<Enemy>();
        if (enemy != null)
        {
            enemy.GetComponent<Enemy>().DoDamage(playerData.attackDamage);
            currentAttackCount++;
        }
        else
        {
            PopEnemy();
        }
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
