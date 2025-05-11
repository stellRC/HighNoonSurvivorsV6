using System.Collections.Generic;
using UnityEngine;

public class MasterAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator masterAnimator;
    private string currentAnimation;
    AnimatorStateInfo animatorStateInfo;

    public List<string> stateAnimation;

    [Header("Combat Animations")]
    public List<string> brawlAnimation;
    public List<string> swordAnimation;
    public List<string> projectileAnimation;
    public List<string> specialAnimation;

    [Header("Movement Animations")]
    public List<string> moveAnimation;
    public List<string> moveSwordAnimation;
    public List<string> moveProjectileAnimation;

    public bool animationFinished;

    private float NTime;

    public bool IsRunning;

    public bool isAttacking;

    public bool isShocking;
    public bool noDamage;

    public bool isGameScene;

    void Awake()
    {
        IsRunning = false;
        isAttacking = false;
        isShocking = false;
    }

    void OnEnable()
    {
        InitAnimationLists();
    }

    private void InitAnimationLists()
    {
        stateAnimation = new List<string>()
        {
            "Die",
            "Stunned",
            "HitDamage",
            "HitDamageUp",
            "Knockback"
        };

        brawlAnimation = new List<string>() { "PunchA", "PunchB", "PunchC", "KickA", "KickB" };
        moveAnimation = new List<string>()
        {
            "Idle",
            "Walk",
            "Run",
            "Sprint",
            "DashLoop",
            "RollLoop"
        };
        projectileAnimation = new List<string>()
        {
            "GunFire",
            "GunFire2H",
            "GunWalkFire",
            "GunRunFire",
            "GunSprintFire",
            "GunCrouchFire"
        };
        moveProjectileAnimation = new List<string>()
        {
            "GunAim",
            "GunWalk",
            "GunRun",
            "GunSprint",
            "GunCrouch",
            "GunReload"
        };
        moveSwordAnimation = new List<string>()
        {
            "SwordIdle",
            "SwordWalk",
            "SwordRun",
            "SwordRunAltGrip",
            "SwordSprint",
            "SwordCrouch"
        };
        swordAnimation = new List<string>()
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

        specialAnimation = new List<string>()
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
        animatorStateInfo = masterAnimator.GetCurrentAnimatorStateInfo(0);
        NTime = animatorStateInfo.normalizedTime;

        if (NTime >= 1.0f)
        {
            animationFinished = true;
        }
        else
        {
            animationFinished = false;
        }

        if (IsRunning)
        {
            masterAnimator.SetBool("IsRunning", true);
        }
        else
        {
            masterAnimator.SetBool("IsRunning", false);
        }

        if (isAttacking)
        {
            isAttacking = false;
            masterAnimator.SetTrigger("IsAttacking");
        }

        if (noDamage)
        {
            Debug.Log("spin");
            masterAnimator.SetBool("IsSpinning", true);
        }
        else
        {
            masterAnimator.SetBool("IsSpinning", false);
        }

        if (isShocking)
        {
            masterAnimator.SetBool("IsShocking", true);
        }
        else
        {
            masterAnimator.SetBool("IsShocking", false);
        }
    }

    // Change animation of enemies and player
    public void ChangeAnimation(string animation, float crossFade = 0.2f)
    {
        if (animation == "Die")
        {
            masterAnimator.SetTrigger("IsDead");
        }
        else if (currentAnimation != animation && animationFinished)
        {
            currentAnimation = animation;
            masterAnimator.CrossFade(animation, crossFade);
        }
    }
}
