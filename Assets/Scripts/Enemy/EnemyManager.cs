using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyData brawlEnemy;

    [SerializeField]
    private EnemyData projectileEnemy;

    [SerializeField]
    private EnemyData rollingEnemy;

    private Vector2 randomPositionOnScreen;

    private int enemySpawnRate;

    private Vector2 lastSpawnPosition;

    private float currentSpawnTime;

    private float projectileSpawnInterval;

    private float brawlerSpawnInterval;

    private float rollerSpawnInterval;

    private float currentWaveTime;
    private float finalWaveTime;

    void Start()
    {
        SpawnMoreEnemies();
        lastSpawnPosition = new Vector2(2, 2);
        projectileSpawnInterval = 10f;
        brawlerSpawnInterval = 15f;
        rollerSpawnInterval = 5f;

        finalWaveTime = 400f;
    }

    void Update()
    {
        currentSpawnTime += Time.deltaTime;
        currentWaveTime += Time.deltaTime;

        if (currentSpawnTime >= projectileSpawnInterval)
        {
            TimedSpawn(projectileEnemy);
        }

        if (currentSpawnTime >= brawlerSpawnInterval)
        {
            TimedSpawn(brawlEnemy);
        }

        if (currentSpawnTime >= rollerSpawnInterval)
        {
            TimedSpawn(rollingEnemy);
        }

        if (GameManager.Instance.clockUI.hoursFloat > 12 && currentWaveTime >= finalWaveTime)
        {
            SpawnFinalWave();
        }
    }

    private void SpawnFinalWave()
    {
        PlaceEnemy(projectileEnemy);
        PlaceEnemy(brawlEnemy);
        PlaceEnemy(rollingEnemy);
        currentWaveTime = 0;
    }

    private void TimedSpawn(EnemyData enemyData)
    {
        PlaceEnemy(enemyData);
        currentSpawnTime = 0;
    }

    public void SpawnMoreEnemies()
    {
        PlaceEnemy(brawlEnemy);
    }

    private void PlaceEnemy(EnemyData enemyData)
    {
        enemySpawnRate = EnemySpawnCount();
        for (int i = 0; i < enemySpawnRate; i++)
        {
            InstantiateEnemy(enemyData);
        }
    }

    // Left bottom corner is 0,0 and right  top corner is 1,1
    private Vector2 RandomScreenCornerPosition()
    {
        randomPositionOnScreen = Camera.main.ViewportToWorldPoint(
            new Vector2(Random.Range(-1.2f, 1.2f), Random.Range(-1.2f, 1.2f))
        );
        var playerPosition = FindAnyObjectByType<PlayerMovement>().transform.position;

        // prevent spawning on top of player
        if (
            lastSpawnPosition == randomPositionOnScreen
            || randomPositionOnScreen == (Vector2)playerPosition
        )
        {
            RandomScreenCornerPosition();
        }
        else
        {
            lastSpawnPosition = randomPositionOnScreen;
        }
        return randomPositionOnScreen;
    }

    private int EnemySpawnCount()
    {
        return Random.Range(1, 2);
    }

    private void InstantiateEnemy(EnemyData enemyData)
    {
        var position = RandomScreenCornerPosition();

        if (enemyData.name == "RollEnemy")
        {
            Debug.Log("spawn roller");
            ObjectPooling.SpawnObject(
                enemyData.enemyPrefab,
                position,
                Quaternion.identity,
                ObjectPooling.PoolType.Rollers
            );
        }
        else if (enemyData.name == "ProjectileEnemyData")
        {
            Debug.Log("spawn projectile man");
            ObjectPooling.SpawnObject(
                enemyData.enemyPrefab,
                position,
                Quaternion.identity,
                ObjectPooling.PoolType.Shooters
            );
        }
        else if (enemyData.name == "BrawlerEnemyData")
        {
            Debug.Log("spawn brawler");
            ObjectPooling.SpawnObject(
                enemyData.enemyPrefab,
                position,
                Quaternion.identity,
                ObjectPooling.PoolType.Brawlers
            );
        }
    }
}
