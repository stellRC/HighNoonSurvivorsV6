using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private EnemyData _enemyData;

    [SerializeField]
    private SpriteRenderer _enemySprite;

    private Transform _playerTransform;

    private MasterAnimator _enemyAnimation;
    public bool IsFacingRight { get; set; }

    private float _distance;
    private bool _isDead;
    private bool _isMovingForward;

    private string _projectileName;

    private float _enemySpeed;

    private Vector2 _randomPosition;
    private Vector2 _currentPosition;

    private void Awake()
    {
        _enemyAnimation = GetComponent<MasterAnimator>();
        _isDead = GetComponent<Enemy>().IsDead;
        _playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        _projectileName = "projectile";
    }

    private void OnEnable()
    {
        InitialTurnCheck();
        _enemyAnimation.ChangeAnimation("Walk");
        _isMovingForward = true;
        _enemySpeed = Random.Range(_enemyData.MoveSpeed, _enemyData.MoveSpeed + 1.5f);
        _randomPosition = RandomScreenPosition();
    }

    private void FixedUpdate()
    {
        if (!MainNavigation.IsPaused)
        {
            // distance between enemy and player
            _distance = Vector2.Distance(transform.position, _playerTransform.position);

            // Direction player is moving in
            Vector2 direction = _playerTransform.position - transform.position;
            direction.Normalize();

            // Rotation angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Flip sprite in direction of player movement
            if (_enemySprite != null)
            {
                TurnCheck();
            }

            // Walk towards player
            if (!_isDead && _enemyAnimation.AnimationFinished)
            {
                EnemyMovementPatterns(_enemyData.MovementPatternID, angle, _enemySpeed, _distance);
            }

            if (GameManager.Instance.PlayerDead)
            {
                StartCoroutine(TriggerDeath(Random.Range(2f, 6f)));
            }
        }
    }

    private void EnemyMovementPatterns(
        int movementPatternID,
        float angle,
        float speed,
        float distance
    )
    {
        switch (movementPatternID)
        {
            case 0:
                RotateTowardsPlayer(angle);
                break;
            case 1:
                MoveTowardsPlayer(speed);
                break;
            case 2:
                ForwardAndRetreat(speed, distance);
                break;
            case 3:
                MoveTo_randomPosition(speed);
                break;
        }
    }

    private void IdleAnimation()
    {
        // Idle animation

        if (_enemyData.EnemyName == _projectileName)
        {
            _enemyAnimation.ChangeAnimation(_enemyAnimation.MoveProjectileAnimation[0]);
        }
        else
        {
            _enemyAnimation.ChangeAnimation(_enemyAnimation.MoveAnimation[0]);
        }
    }

    private void RunAnimation()
    {
        if (_enemyData.EnemyName == _projectileName)
        {
            _enemyAnimation.ChangeAnimation(_enemyAnimation.MoveProjectileAnimation[2]);
        }
        else
        {
            _enemyAnimation.ChangeAnimation(_enemyAnimation.MoveAnimation[2]);
        }
    }

    private void RotateTowardsPlayer(float angle)
    {
        RunAnimation();

        // Create a smooth updating turn rotation
        transform.SetPositionAndRotation(
            Vector2.MoveTowards(
                this.transform.position,
                _playerTransform.position,
                _enemyData.MoveSpeed * Time.deltaTime
            ),
            Quaternion.Euler(Vector3.forward * angle)
        );
    }

    // Move towards player and then retreat if player moves towards enemy
    private void ForwardAndRetreat(float speed, float distanceBetween)
    {
        if (_isMovingForward)
        {
            MoveTowardsPlayer(speed);
            if (distanceBetween < _enemyData.AttackDistance)
            {
                _isMovingForward = false;
            }
        }
        else
        {
            // Retreat from player if too close
            if (distanceBetween < _enemyData.MinimumDistance)
            {
                MoveTowardsPlayer(-speed);
            }
            else
            {
                IdleAnimation();
            }
        }
    }

    // Check if reached target position, continue if haven't and change target if have;
    private void MoveTo_randomPosition(float speed)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            _randomPosition,
            Time.deltaTime * speed
        );
        if (transform.CompareTag("roller"))
        {
            RollAnimation();
            // RollingSound();
            _currentPosition = transform.position;
            if (_currentPosition == _randomPosition)
            {
                StartCoroutine(TriggerDeath(Random.Range(3f, 10f)));
            }
        }
        else
        {
            RunAnimation();

            _currentPosition = transform.position;
            if (_currentPosition == _randomPosition)
            {
                DeathAnimation();
            }
        }

        // Return roller enemy to pool after reached target position, stats not updated
    }

    IEnumerator TriggerDeath(float count)
    {
        yield return new WaitForSeconds(count);
        DeathAnimation();
    }

    private void DeathAnimation()
    {
        // state animation 4 = knockback
        _enemyAnimation.ChangeAnimation(_enemyAnimation.StateAnimation[4]);
    }

    private void RollAnimation()
    {
        _enemyAnimation.ChangeAnimation(_enemyAnimation.MoveAnimation[5]);
    }

    private void RollingSound()
    {
        SoundEffectsManager.Instance.PlayRandomSoundFXClip(
            SoundEffectsManager.Instance.rollingSoundClips,
            transform,
            .5f
        );
    }

    // random position ANYWHERE on screen
    private Vector2 RandomScreenPosition()
    {
        return Camera.main.ViewportToWorldPoint(
            new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f))
        );
    }

    // Without rotation
    private void MoveTowardsPlayer(float speed)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            _playerTransform.position,
            speed * Time.deltaTime
        );
        RunAnimation();
    }

    private void TurnCheck()
    {
        if (_playerTransform.position.x > transform.position.x && !IsFacingRight)
        {
            Turn();
        }
        else if (_playerTransform.position.x < transform.position.x && IsFacingRight)
        {
            Turn();
        }
    }

    private void InitialTurnCheck()
    {
        if (_playerTransform.position.x < transform.position.x)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (IsFacingRight)
        {
            transform.rotation = Quaternion.Euler(
                new Vector3(transform.rotation.x, 180f, transform.rotation.z)
            );
            IsFacingRight = !IsFacingRight;
        }
        else
        {
            transform.rotation = Quaternion.Euler(
                new Vector3(transform.rotation.x, 0f, transform.rotation.z)
            );
            IsFacingRight = !IsFacingRight;
        }
    }
}
