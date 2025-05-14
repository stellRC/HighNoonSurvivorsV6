using System.Collections;
using DigitalRuby.LightningBolt;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    // Components
    private PlayerData _playerData;
    private MasterAnimator playerAnimator;
    private CinemachineImpulseSource _impulseSource;
    private SkillTreeManager _skillTreeManager;
    private CooldownBar _cooldownBar;

    [Header("Combat")]
    [SerializeField]
    private Transform _attackPoint;

    [SerializeField]
    public LayerMask EnemyLayers;

    [Header("Input")]
    [SerializeField]
    private InputActionReference _normalAttack;

    [SerializeField]
    private InputActionReference _specialAttack;

    // Private variables

    private int _chosenSpecialMove;

    private bool _canUseSpecial;

    private int _currentAttackCount;

    private float _attackRange;

    private float _popEnemyTime;

    private bool _isPopping;

    private void Awake()
    {
        playerAnimator = GetComponent<MasterAnimator>();
        _skillTreeManager = FindFirstObjectByType<SkillTreeManager>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _playerData = GetComponent<PlayerController>().PlayerData;
    }

    private void Start()
    {
        _attackRange = _playerData.AttackRange;
        _canUseSpecial = false;

        _chosenSpecialMove = _skillTreeManager.chosenSpecialMove;
        _cooldownBar = FindFirstObjectByType<CooldownBar>();
        _currentAttackCount = 0;
    }

    // Input messaging, prevent accidental multiple attacks
    void OnEnable()
    {
        _normalAttack.action.started += NormalAttack;
        _specialAttack.action.started += SpecialAttack;
    }

    void OnDisable()
    {
        _normalAttack.action.started -= NormalAttack;
        _specialAttack.action.started -= SpecialAttack;
    }

    void Update()
    {
        // If number of killed enemies is == 10, special attack is true
        // Each enemy kill fills the bar
        // If special attack is used, drain bar and reset attack number needed
        if (_currentAttackCount >= GameManager.Instance.LevelData.specialAttackRate)
        {
            _canUseSpecial = true;
        }
        else
        {
            _canUseSpecial = false;
        }

        if (_isPopping)
        {
            _popEnemyTime += Time.deltaTime;
            if (_popEnemyTime >= 1)
            {
                PopEnemy();
                _popEnemyTime = 0;
            }
        }
    }

    private void PlayerAttackAudio()
    {
        SoundEffectsManager.instance.PlayRandomSoundFXClip(
            SoundEffectsManager.instance.normalPlayerAttackSingleSoundClips,
            transform,
            1f
        );
    }

    public void NormalAttack(InputAction.CallbackContext context)
    {
        if (!_skillTreeManager.isSpecialAnim)
        {
            playerAnimator.IsAttacking = true;

            Attack();
            PlayerAttackAudio();
        }
    }

    public void SpecialAttack(InputAction.CallbackContext context)
    {
        // mouse up

        if (_canUseSpecial && context.started && _chosenSpecialMove >= 0)
        {
            playerAnimator.ChangeAnimation(playerAnimator.SpecialAnimation[_chosenSpecialMove]);
            CheckSpecialMove(_chosenSpecialMove);
            Attack();
            _cooldownBar.StartDrain = true;
            _currentAttackCount = 0;
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
        _attackRange = _playerData.SpecialAttackRange;
        CameraShake.instance.ScreenShakeFromProfile(3, _impulseSource);
        StartCoroutine(ToggleSwordCombo(GameManager.Instance.LevelData.specialAttackRate));
    }

    // Player becomes impervious to damage
    private void TriggerSpin()
    {
        GameManager.Instance.NoDamage = true;
        playerAnimator.noDamage = true;
        CameraShake.instance.ScreenShakeFromProfile(1, _impulseSource);
        StartCoroutine(ToggleSpin(GameManager.Instance.LevelData.specialAttackRate));
    }

    // Destroy enemies positioned between player and clock
    private void TriggerLightning()
    {
        LightningBoltScript lightningBoltScript = FindFirstObjectByType<LightningBoltScript>();
        lightningBoltScript.ManualMode = false;
        playerAnimator.IsShocking = true;
        CameraShake.instance.ScreenShakeFromProfile(0, _impulseSource);
        StartCoroutine(
            ToggleLightning(GameManager.Instance.LevelData.specialAttackRate, lightningBoltScript)
        );
    }

    // Randomly destroy enemies every second for length of special attack
    private void TriggerFog()
    {
        FogController collisionFog = FindAnyObjectByType<FogController>();
        collisionFog.IsPlaying = true;
        _isPopping = true;
        CameraShake.instance.ScreenShakeFromProfile(2, _impulseSource);
        StartCoroutine(ToggleFog(GameManager.Instance.LevelData.specialAttackRate, collisionFog));
    }

    IEnumerator ToggleLightning(float count, LightningBoltScript lightningBoltScript)
    {
        yield return new WaitForSeconds(count);

        lightningBoltScript.ManualMode = true;
        playerAnimator.IsShocking = false;
    }

    IEnumerator ToggleSpin(float count)
    {
        yield return new WaitForSeconds(count);

        GameManager.Instance.NoDamage = false;
        playerAnimator.noDamage = false;
    }

    IEnumerator ToggleSwordCombo(float count)
    {
        yield return new WaitForSeconds(count);
        _attackRange = _playerData.AttackRange;
    }

    IEnumerator ToggleFog(float count, FogController collisionFog)
    {
        yield return new WaitForSeconds(count);
        collisionFog.IsPlaying = false;
        _isPopping = false;
    }

    private void PopEnemy()
    {
        // enemy pop time is one for every second in special attack count
        var enemy = FindFirstObjectByType<Enemy>();
        if (enemy != null)
        {
            enemy.GetComponent<Enemy>().DoDamage(_playerData.AttackDamage);
            _currentAttackCount++;
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
            _attackPoint.position,
            _attackRange,
            EnemyLayers
        );

        // Damage Enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().DoDamage(_playerData.AttackDamage);
                _currentAttackCount++;
                _cooldownBar.RefillCooldown();
            }
        }
    }
}
