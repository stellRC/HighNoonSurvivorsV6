using System.Collections.Generic;
using UnityEngine;

public class MasterAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator _masterAnimator;
    private string _currentAnimation;
    AnimatorStateInfo _animatorStateInfo;

    public List<string> StateAnimation;

    [Header("Combat Animations")]
    public List<string> BrawlAnimation;
    public List<string> SwordAnimation;
    public List<string> ProjectileAnimation;
    public List<string> SpecialAnimation;

    [Header("Movement Animations")]
    public List<string> MoveAnimation;
    public List<string> MoveSwordAnimation;
    public List<string> MoveProjectileAnimation;

    public bool AnimationFinished;

    public bool IsRunning;

    public bool IsAttacking;

    public bool IsShocking;
    public bool noDamage;

    public bool IsGameScene;
    private float _normalizedTime;

    void Awake()
    {
        IsRunning = false;
        IsAttacking = false;
        IsShocking = false;
    }

    void OnEnable()
    {
        InitAnimationLists();
    }

    private void InitAnimationLists()
    {
        StateAnimation = new List<string>()
        {
            "Die",
            "Stunned",
            "HitDamage",
            "HitDamageUp",
            "Knockback"
        };

        BrawlAnimation = new List<string>() { "PunchA", "PunchB", "PunchC", "KickA", "KickB" };
        MoveAnimation = new List<string>()
        {
            "Idle",
            "Walk",
            "Run",
            "Sprint",
            "DashLoop",
            "RollLoop"
        };
        ProjectileAnimation = new List<string>()
        {
            "GunFire",
            "GunFire2H",
            "GunWalkFire",
            "GunRunFire",
            "GunSprintFire",
            "GunCrouchFire"
        };
        MoveProjectileAnimation = new List<string>()
        {
            "GunAim",
            "GunWalk",
            "GunRun",
            "GunSprint",
            "GunCrouch",
            "GunReload"
        };
        MoveSwordAnimation = new List<string>()
        {
            "SwordIdle",
            "SwordWalk",
            "SwordRun",
            "SwordRunAltGrip",
            "SwordSprint",
            "SwordCrouch"
        };
        SwordAnimation = new List<string>()
        {
            "SwordGuard",
            "SwordGuardImpact",
            "SwordAttack",
            "SwordStandingSlash",
            "SwordSprintSlash",
            "CrouchSlash",
            "SwordRunSlash",
            "ComboAttackA",
            "ComboAttackB",
            "ComboAttackD"
        };

        SpecialAnimation = new List<string>()
        {
            "Spin",
            "ComboAttackA",
            "ShockHeavy",
            "GroundSlam"
        };
    }

    // Triggers and bools align with those in inspector
    void Update()
    {
        _animatorStateInfo = _masterAnimator.GetCurrentAnimatorStateInfo(0);
        _normalizedTime = _animatorStateInfo.normalizedTime;

        if (_normalizedTime >= 1.0f)
        {
            AnimationFinished = true;
        }
        else
        {
            AnimationFinished = false;
        }

        if (IsRunning)
        {
            _masterAnimator.SetBool("IsRunning", true);
        }
        else
        {
            _masterAnimator.SetBool("IsRunning", false);
        }

        if (IsAttacking)
        {
            IsAttacking = false;
            _masterAnimator.SetTrigger("IsAttacking");
        }

        if (noDamage)
        {
            Debug.Log("spin");
            _masterAnimator.SetBool("IsSpinning", true);
        }
        else
        {
            _masterAnimator.SetBool("IsSpinning", false);
        }

        if (IsShocking)
        {
            _masterAnimator.SetBool("IsShocking", true);
        }
        else
        {
            _masterAnimator.SetBool("IsShocking", false);
        }
    }

    // Change animation of enemies and player
    public void ChangeAnimation(string animation, float crossFade = 0.2f)
    {
        if (animation == "Die")
        {
            _masterAnimator.SetTrigger("IsDead");
        }
        else if (_currentAnimation != animation && AnimationFinished)
        {
            _currentAnimation = animation;
            _masterAnimator.CrossFade(animation, crossFade);
        }
    }
}
