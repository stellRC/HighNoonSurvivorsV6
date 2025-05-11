using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CameraFollowObject cameraFollowObject;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private Animator fxAnimator;

    public MasterAnimator playerAnimator;
    private Rigidbody2D playerRigidBody;

    public bool IsFacingRight { get; set; }

    private bool isDashing;

    private bool isMoving;
    private Vector2 moveInput;

    //DASH
    private float activeMoveSpeed;
    private float dashSpeed;

    private float breathingCount;

    private float breathingWait;
    private float runningWait;
    private float runningCount;

    private void Awake()
    {
        playerAnimator = GetComponent<MasterAnimator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        cameraFollowObject = FindAnyObjectByType<CameraFollowObject>();
        breathingWait = 100f;
    }

    void Start()
    {
        isDashing = false;
        IsFacingRight = true;

        isMoving = false;

        activeMoveSpeed = moveSpeed;
        dashSpeed = 7f;
        playerAnimator.ChangeAnimation("SwordIdle");

        TurnCheck(moveInput);
    }

    private void Update()
    {
        breathingCount += Time.deltaTime;
        runningCount += Time.deltaTime;
        // Idle
        if (!isDashing & !isMoving)
        {
            playerAnimator.ChangeAnimation(playerAnimator.moveSwordAnimation[0]);
            activeMoveSpeed = moveSpeed;
            playerAnimator.IsRunning = false;
            fxAnimator.gameObject.SetActive(false);

            if (breathingCount >= breathingWait)
            {
                breathingCount = 0;
                breathingWait = UnityEngine.Random.Range(100f, 500f);
                IdleSFX();
            }
        }

        // Running
        if (!isDashing & isMoving)
        {
            playerAnimator.ChangeAnimation(playerAnimator.moveSwordAnimation[2]);
            activeMoveSpeed = moveSpeed;

            if (runningCount >= runningWait)
            {
                runningCount = 0;
                runningWait = UnityEngine.Random.Range(50f, 200f);
                RunningSFX();
            }
        }
    }

    private void RunningSFX()
    {
        SoundEffectsManager.instance.PlayRandomSoundFXClip(
            SoundEffectsManager.instance.playerRunningClips,
            transform,
            .3f
        );
    }

    private void IdleSFX()
    {
        SoundEffectsManager.instance.PlayRandomSoundFXClip(
            SoundEffectsManager.instance.playerIdleClips,
            transform,
            .1f
        );
    }

    private void DashingSFX()
    {
        SoundEffectsManager.instance.PlayRandomSoundFXClip(
            SoundEffectsManager.instance.playerDashingClips,
            transform,
            .5f
        );
    }

    //good to use fixed update for rigid body objects
    void FixedUpdate()
    {
        playerRigidBody.linearVelocity = moveInput * activeMoveSpeed;

        if (playerRigidBody.linearVelocityX == 0 & playerRigidBody.linearVelocityY == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;

            playerAnimator.IsRunning = true;
            fxAnimator.gameObject.SetActive(true);
        }

        if (moveInput.x > 0 || moveInput.x < 0)
        {
            TurnCheck(moveInput);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        var currentInput = context.ReadValue<Vector2>();
        if (currentInput != moveInput)
        {
            moveInput = currentInput;
        }
    }

    public void TurnCheck(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            Turn();
        }
        else if (moveInput.x <= 0 && IsFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (IsFacingRight)
        {
            Vector3 rotator = new(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            //turn camera follow object
            cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            //turn camera follow object
            cameraFollowObject.CallTurn();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isDashing = !isDashing;
            DashingSFX();
            playerAnimator.ChangeAnimation(playerAnimator.moveAnimation[4]);
            fxAnimator.SetBool("IsDashing", true);
            activeMoveSpeed = dashSpeed;
        }

        if (context.canceled)
        {
            fxAnimator.SetBool("IsDashing", false);
            isDashing = !isDashing;
        }
    }
}
