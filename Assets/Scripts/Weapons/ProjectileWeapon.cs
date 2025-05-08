using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;

    public void InstantiateProjectile(Vector2 spawnPosition)
    {
        ObjectPooling.SpawnObject(
            enemyData.projectilePrefab,
            spawnPosition,
            Quaternion.identity,
            ObjectPooling.PoolType.Projectiles
        );
    }
}
