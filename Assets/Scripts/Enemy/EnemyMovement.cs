using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private SpriteRenderer enemySprite;

    [SerializeField]
    private AudioClip[] rollingSoundClips;

    private Transform playerTransform;

    private MasterAnimator enemyAnimation;
    public bool IsFacingRight { get; set; }

    private float distance;
    bool isDead;
    private bool isMovingForward;

    private string projectileName;

    private float enemySpeed;

    private Vector2 randomPosition;
    private Vector2 currentPosition;

    private Vector2 targetPosition;

    private void Awake()
    {
        enemyAnimation = GetComponent<MasterAnimator>();
        isDead = GetComponent<Enemy>().isDead;
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        projectileName = "projectile";
    }

    private void OnEnable()
    {
        InitialTurnCheck();
        enemyAnimation.ChangeAnimation("Walk");
        isMovingForward = true;
        enemySpeed = Random.Range(enemyData.moveSpeed, enemyData.moveSpeed + 1.5f);
        randomPosition = RandomScreenPosition();
    }

    private void FixedUpdate()
    {
        if (!MainNavigation.isPaused)
        {
            // distance between enemy and player
            distance = Vector2.Distance(transform.position, playerTransform.position);

            // Direction player is moving in
            Vector2 direction = playerTransform.position - transform.position;
            direction.Normalize();

            // Rotation angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Flip sprite in direction of player movement
            if (enemySprite != null)
            {
                TurnCheck();
            }

            // Walk towards player
            if (!isDead && enemyAnimation.animationFinished)
            {
                EnemyMovementPatterns(enemyData.movementPatternID, angle, enemySpeed, distance);
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
                MoveToRandomPosition(speed);
                break;
        }
    }

    private void IdleAnimation()
    {
        // Idle animation

        if (enemyData.EnemyName == projectileName)
        {
            enemyAnimation.ChangeAnimation(enemyAnimation.moveProjectileAnimation[0]);
        }
        else
        {
            enemyAnimation.ChangeAnimation(enemyAnimation.moveAnimation[0]);
        }
    }

    private void RunAnimation()
    {
        if (enemyData.EnemyName == projectileName)
        {
            enemyAnimation.ChangeAnimation(enemyAnimation.moveProjectileAnimation[2]);
        }
        else
        {
            enemyAnimation.ChangeAnimation(enemyAnimation.moveAnimation[2]);
        }
    }

    private void RotateTowardsPlayer(float angle)
    {
        RunAnimation();

        // Create a smooth updating turn rotation
        transform.SetPositionAndRotation(
            Vector2.MoveTowards(
                this.transform.position,
                playerTransform.position,
                enemyData.moveSpeed * Time.deltaTime
            ),
            Quaternion.Euler(Vector3.forward * angle)
        );
    }

    // Move towards player and then retreat if player moves towards enemy
    private void ForwardAndRetreat(float speed, float distanceBetween)
    {
        // if (enemyAnimation.animationFinished)
        // {
        if (isMovingForward)
        {
            MoveTowardsPlayer(speed);
            if (distanceBetween < enemyData.attackDistance)
            {
                isMovingForward = false;
            }
        }
        else
        {
            // Retreat from player if too close
            if (distanceBetween < enemyData.minimumDistance)
            {
                MoveTowardsPlayer(-speed);
            }
            else
            {
                IdleAnimation();
            }
        }
        // if (distanceBetween > enemyData.attackDistance)
        // {
        //     isMovingForward = true;
        // }
        // }
    }

    // Check if reached target position, continue if haven't and change target if have;
    private void MoveToRandomPosition(float speed)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            randomPosition,
            Time.deltaTime * speed
        );

        RollAnimation();

        // Return roller enemy to pool after reached target position, stats not updated
        currentPosition = transform.position;
        if (currentPosition == randomPosition)
        {
            StartCoroutine(NewRollPosition(Random.Range(3f, 10f)));
        }
    }

    IEnumerator NewRollPosition(float count)
    {
        yield return new WaitForSeconds(count);
        DeathAnimation();
    }

    private void DeathAnimation()
    {
        // state animation 4 = knockback
        enemyAnimation.ChangeAnimation(enemyAnimation.stateAnimation[4]);
    }

    private void RollAnimation()
    {
        enemyAnimation.ChangeAnimation(enemyAnimation.moveAnimation[5]);
        SoundEffectsManager.instance.PlayRandomSoundFXClip(rollingSoundClips, transform, 1f);
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
            playerTransform.position,
            speed * Time.deltaTime
        );
        RunAnimation();
    }

    private void TurnCheck()
    {
        if (playerTransform.position.x > transform.position.x && !IsFacingRight)
        {
            Turn();
        }
        else if (playerTransform.position.x < transform.position.x && IsFacingRight)
        {
            Turn();
        }
    }

    private void InitialTurnCheck()
    {
        if (playerTransform.position.x < transform.position.x)
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
