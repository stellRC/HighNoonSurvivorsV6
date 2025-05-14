using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _playerRigidBody;
    private PlayerData _playerData;
    private CameraFollowObject _cameraFollowObject;

    [Header("Animators")]
    [SerializeField]
    private Animator _fxAnimator;

    public MasterAnimator PlayerAnimator;

    [Header("Input")]
    [SerializeField]
    private InputActionReference move;

    [SerializeField]
    private InputActionReference dash;
    private Vector2 _moveDirection;

    public bool IsFacingRight { get; set; }

    private bool _isMoving;
    private bool _isDashing;

    [Header("Speed")]
    [SerializeField]
    private float _activeMoveSpeed;

    [Header("SFX")]
    private float _sFXWait;
    private float _breathingCount;
    private float _runningCount;

    private void Awake()
    {
        PlayerAnimator = GetComponent<MasterAnimator>();
        _playerRigidBody = GetComponent<Rigidbody2D>();
        _cameraFollowObject = FindAnyObjectByType<CameraFollowObject>();
        _playerData = GetComponent<PlayerController>().PlayerData;
    }

    private void Start()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        // Start off idling
        _isMoving = false;
        _isDashing = false;
        IsFacingRight = true;
        _sFXWait = 100f;

        _activeMoveSpeed = _playerData.MoveSpeed;

        PlayerAnimator.ChangeAnimation("SwordIdle");

        TurnCheck(_moveDirection);
    }

    // Input messaging
    void OnEnable()
    {
        dash.action.started += Dash;
        dash.action.canceled += DashCancelled;
    }

    void OnDisable()
    {
        dash.action.started -= Dash;
        dash.action.canceled -= DashCancelled;
    }

    private void Update()
    {
        // WASD or Arrow keys
        _moveDirection = move.action.ReadValue<Vector2>();

        // SFX
        _breathingCount += Time.deltaTime;
        _runningCount += Time.deltaTime;

        // Set movement state
        if (_playerRigidBody.linearVelocityX == 0 & _playerRigidBody.linearVelocityY == 0)
        {
            _isMoving = false;
        }
        else
        {
            _isMoving = true;
        }

        // Change movement SFX, animation, bools
        if (!_isDashing)
        {
            if (!_isMoving)
            {
                IdleMovement();
            }
            else
            {
                RunningMovement();
            }
        }
        else
        {
            RunningOrIdleDashing();

            if (_isDashing & !_isMoving)
            {
                IdleDashing();
            }
        }

        // Turn player in direction nof movement
        if (_moveDirection.x > 0 || _moveDirection.x < 0)
        {
            TurnCheck(_moveDirection);
        }

        // Update FX animator state
        FxAnimator();
    }

    //good to use fixed update for rigid body objects
    void FixedUpdate()
    {
        // Move player idle dashing
        if (_isDashing && !_isMoving)
        {
            _playerRigidBody.linearVelocity =
                new Vector2(_moveDirection.x, _moveDirection.y).normalized * _activeMoveSpeed;
        }
        // Move player dashing and running
        else
        {
            _playerRigidBody.linearVelocity = new Vector2(
                _moveDirection.x * _activeMoveSpeed,
                _moveDirection.y * _activeMoveSpeed
            );
        }
    }

    private void IdleMovement()
    {
        PlayerAnimator.ChangeAnimation(PlayerAnimator.MoveSwordAnimation[0]);

        PlayerAnimator.IsRunning = false;

        // SFX CONTROLLER
        if (_breathingCount >= _sFXWait)
        {
            _breathingCount = 0;
            _sFXWait = UnityEngine.Random.Range(100f, 500f);
            IdleSFX();
        }
    }

    private void RunningMovement()
    {
        PlayerAnimator.IsRunning = true;
        PlayerAnimator.ChangeAnimation(PlayerAnimator.MoveSwordAnimation[2], 0);
        _activeMoveSpeed = _playerData.MoveSpeed;

        // SFX CONTROLLER
        if (_runningCount >= _sFXWait)
        {
            _runningCount = 0;
            _sFXWait = UnityEngine.Random.Range(50f, 200f);
            RunningSFX();
        }
    }

    // Must change in update or won't always transition to dashing animation
    private void RunningOrIdleDashing()
    {
        // GameManager.Instance.NoDamage = true;
        PlayerAnimator.ChangeAnimation(PlayerAnimator.MoveAnimation[4]);
    }

    private void IdleDashing()
    {
        if (_activeMoveSpeed != _playerData.DashSpeed)
        {
            _activeMoveSpeed = _playerData.DashSpeed;
        }

        // Dash in forward direction
        if (IsFacingRight == true)
        {
            _moveDirection.x = transform.localScale.x;
        }
        else
        {
            _moveDirection.x = -transform.localScale.x;
        }

        _moveDirection.y = 0;
    }

    private void FxAnimator()
    {
        if (_isDashing || _isMoving && !_fxAnimator.gameObject.activeSelf)
        {
            _fxAnimator.gameObject.SetActive(true);
        }
        else
        {
            _fxAnimator.gameObject.SetActive(false);
        }
    }

    public void TurnCheck(Vector2 _moveDirection)
    {
        if (_moveDirection.x > 0 && !IsFacingRight)
        {
            Turn();
        }
        else if (_moveDirection.x <= 0 && IsFacingRight)
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
            _cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            //turn camera follow object
            _cameraFollowObject.CallTurn();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        _isDashing = true;
        _fxAnimator.SetBool("IsDashing", true);

        // Change dash speed (around 2.5 x regular speed)
        _activeMoveSpeed = _playerData.DashSpeed;

        // Play Dashing SFX once
        DashingSFX();
    }

    private void DashCancelled(InputAction.CallbackContext context)
    {
        _isDashing = false;

        _fxAnimator.SetBool("IsDashing", false);
    }

    // SFX initial volume varies to add layers to sounds effects
    private void RunningSFX()
    {
        SoundEffectsManager.Instance.PlayRandomSoundFXClip(
            SoundEffectsManager.Instance.playerRunningClips,
            transform,
            .3f
        );
    }

    private void IdleSFX()
    {
        SoundEffectsManager.Instance.PlayRandomSoundFXClip(
            SoundEffectsManager.Instance.playerIdleClips,
            transform,
            .1f
        );
    }

    private void DashingSFX()
    {
        SoundEffectsManager.Instance.PlayRandomSoundFXClip(
            SoundEffectsManager.Instance.playerDashingClips,
            transform,
            .5f
        );
    }
}
