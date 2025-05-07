using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private SpriteRenderer enemySprite;

    private Transform playerTransform;

    // private GunAnimation enemyAnimation;
    private MasterAnimator enemyAnimation;
    public bool IsFacingRight { get; set; }

    private float distance;
    bool isDead;
    private bool isMovingForward;

    // private float retreatTime;

    private string projectileName;

    private void Awake()
    {
        // enemyAnimation = GetComponent<GunAnimation>();
        enemyAnimation = GetComponent<MasterAnimator>();
        isDead = GetComponent<Enemy>().isDead;
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
        projectileName = "projectile";
        // retreatTime = 3f;
    }

    private void OnEnable()
    {
        InitialTurnCheck();
        enemyAnimation.ChangeAnimation("Walk");
        isMovingForward = true;
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
                EnemyMovementPatterns(
                    enemyData.movementPatternID,
                    angle,
                    enemyData.moveSpeed,
                    distance
                );
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
