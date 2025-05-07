using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private Transform attackPoint;

    private Vector2 spawnPosition;
    private MasterAnimator enemyAnimation;
    private Transform playerTransform;

    private float timeBetweenShots = 0;
    private float nextShotTime;

    // private Vector3 offset;

    // private int bulletCount;

    void Awake()
    {
        enemyAnimation = GetComponent<MasterAnimator>();
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (
            Vector2.Distance(transform.position, playerTransform.position)
            <= enemyData.minimumDistance
        )
        {
            // Check if can shoot
            if (Time.time > nextShotTime)
            {
                var attack = Random.Range(0, 5);
                // Shoot animation
                enemyAnimation.ChangeAnimation(enemyAnimation.projectileAnimation[attack]);

                // Reset timer
                timeBetweenShots = Random.Range(1.5f, 5.0f);
                nextShotTime = Time.time + timeBetweenShots;
            }
        }

        // Extremely jacky way to spawn bullets in correct position
    }

    public void InstantiateProjectile(Vector2 spawnPoint)
    {
        // spawnPosition = new Vector2(
        //     transform.position.x + 0.1f,
        //     transform.position.y + 0.5f + (transform.position.y / 2)
        // );
        ObjectPooling.SpawnObject(
            enemyData.projectilePrefab,
            spawnPoint,
            Quaternion.identity,
            ObjectPooling.PoolType.Projectiles
        );
    }
}
