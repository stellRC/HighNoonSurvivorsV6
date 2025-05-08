using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;

    public void InstantiateProjectile()
    {
        ObjectPooling.SpawnObject(
            enemyData.projectilePrefab,
            transform.position,
            Quaternion.identity,
            ObjectPooling.PoolType.Projectiles
        );
    }
}
