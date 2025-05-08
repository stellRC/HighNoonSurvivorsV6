using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CameraFollowObject cameraFollowObject;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private Animator fxAnimator;

    private MasterAnimator playerAnimator;
    private Rigidbody2D playerRigidBody;

    public bool IsFacingRight { get; set; }

    private bool isDashing;

    private bool isMoving;
    private Vector2 moveInput;

    //DASH
    private float activeMoveSpeed;
    private float dashSpeed;

    private void Awake()
    {
        playerAnimator = GetComponent<MasterAnimator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        cameraFollowObject = FindAnyObjectByType<CameraFollowObject>();
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
        // Idle
        if (!isDashing & !isMoving)
        {
            playerAnimator.ChangeAnimation(playerAnimator.moveSwordAnimation[0]);
            activeMoveSpeed = moveSpeed;
            playerAnimator.IsRunning = false;
            fxAnimator.gameObject.SetActive(false);
        }

        // Running
        if (!isDashing & isMoving)
        {
            playerAnimator.ChangeAnimation(playerAnimator.moveSwordAnimation[2]);
            activeMoveSpeed = moveSpeed;
        }
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
        moveInput = context.ReadValue<Vector2>();
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
