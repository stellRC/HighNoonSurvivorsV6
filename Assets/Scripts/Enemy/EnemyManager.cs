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
        projectileSpawnInterval = 20f;
        brawlerSpawnInterval = 10f;
        rollerSpawnInterval = 15f;

        finalWaveTime = 2f;
    }

    void Update()
    {
        currentSpawnTime += Time.deltaTime;
        currentWaveTime += Time.deltaTime;

        if (currentSpawnTime >= projectileSpawnInterval)
        {
            TimedSpawn(projectileEnemy, projectileSpawnInterval);
        }

        if (currentSpawnTime >= brawlerSpawnInterval)
        {
            TimedSpawn(brawlEnemy, brawlerSpawnInterval);
        }

        if (currentSpawnTime >= rollerSpawnInterval)
        {
            TimedSpawn(rollingEnemy, rollerSpawnInterval);
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
        currentWaveTime = 0;
    }

    private void TimedSpawn(EnemyData enemyData, float spawnInterval)
    {
        PlaceEnemy(enemyData);
        currentSpawnTime = 0;
        spawnInterval = Random.Range(10f, 25f);
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

    // random position ANYWHERE on screen
    private void RandomScreenPosition()
    {
        randomPositionOnScreen = Camera.main.ViewportToWorldPoint(
            new Vector2(Random.value, Random.value)
        );
    }

    // Left bottom corner is 0,0 and right  top corner is 1,1
    private Vector2 RandomScreenCornerPosition()
    {
        randomPositionOnScreen = Camera.main.ViewportToWorldPoint(
            new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f))
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
        return Random.Range(1, 3);
    }

    private void InstantiateEnemy(EnemyData enemyData)
    {
        var position = RandomScreenCornerPosition();

        ObjectPooling.SpawnObject(
            enemyData.enemyPrefab,
            position,
            Quaternion.identity,
            ObjectPooling.PoolType.Enemies
        );
    }
}
