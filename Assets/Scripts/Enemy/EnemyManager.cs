using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyData brawlEnemy;

    [SerializeField]
    private EnemyData projectileEnemy;

    private Vector2 randomPositionOnScreen;

    private int enemySpawnRate;

    private Vector2 lastSpawnPosition;

    void Start()
    {
        SpawnMoreEnemies();
        lastSpawnPosition = new Vector2(2, 2);
    }

    public void SpawnMoreEnemies()
    {
        PlaceEnemy(brawlEnemy);
        PlaceEnemy(projectileEnemy);
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
            new Vector2(Random.Range(0f, 2f), Random.Range(0f, 2f))
        );
        if (lastSpawnPosition == randomPositionOnScreen)
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
