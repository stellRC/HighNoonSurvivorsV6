using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    // create new object pool for ever game object passed into it
    private GameObject objectPoolEmptyHolder;
    private static GameObject particleSystemEmpty;
    private static GameObject projectilesEmpty;
    private static GameObject enemiesEmpty;
    private static GameObject brawlersEmpty;
    private static GameObject shootersEmpty;
    private static GameObject rollersEmpty;

    private static GameObject audioSourceEmpty;

    private int projectileMaxCount;

    private int enemyMaxCount;

    public enum PoolType
    {
        ParticleSystem,
        Projectiles,

        // Enemies,
        Brawlers,
        Shooters,
        Rollers,
        Audio,
        None
    }

    private void Awake()
    {
        SetupEmpty();

        enemyMaxCount = 15;
        projectileMaxCount = 40;
    }

    private void SetupEmpty()
    {
        objectPoolEmptyHolder = new GameObject("Pooled Objects");

        particleSystemEmpty = new GameObject("Particle Effects");
        particleSystemEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        projectilesEmpty = new GameObject("Projectile Objects");
        projectilesEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        enemiesEmpty = new GameObject("Enemy Objects");
        enemiesEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        brawlersEmpty = new GameObject("Brawler Objects");
        brawlersEmpty.transform.SetParent(enemiesEmpty.transform);

        shootersEmpty = new GameObject("Shooter Objects");
        shootersEmpty.transform.SetParent(enemiesEmpty.transform);

        rollersEmpty = new GameObject("Roller Objects");
        rollersEmpty.transform.SetParent(enemiesEmpty.transform);

        audioSourceEmpty = new GameObject("Audio Source Objects");
        audioSourceEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
    }

    public static PoolType PoolingType;

    public static List<PooledObjectInfo> ObjectPools = new();

    private void Update()
    {
        CullPool(projectilesEmpty, projectileMaxCount);

        CullPool(brawlersEmpty, enemyMaxCount);
        CullPool(shootersEmpty, enemyMaxCount);
        CullPool(rollersEmpty, enemyMaxCount);

        CullAudioPool(audioSourceEmpty);
    }

    private void CullPool(GameObject pool, int maxPoolCount)
    {
        if (
            pool != null
            && pool.transform.childCount > 0
            && pool.transform.childCount > maxPoolCount
        )
        {
            ReturnObjectToPool(pool.transform.GetChild(0).gameObject);
        }
    }

    private void CullAudioPool(GameObject pool)
    {
        foreach (Transform child in pool.transform)
        {
            if (!child.GetComponent<AudioSource>().isPlaying)
            {
                ReturnObjectToPool(child.gameObject);
            }
        }
    }

    public static GameObject SpawnObject(
        GameObject objectToSpawn,
        Vector3 spawnPosition,
        Quaternion spawnRotation,
        PoolType poolType = PoolType.None
    )
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        // If pool doesn't exist
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        // Check if there are any inactive objects in the pool
        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            GameObject parentObject = SetParentObject(poolType);
            // If there are no inactive objects, create a new one
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            // If there is an inactive object, reactivate it
            spawnableObj.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        // Remove (Clone) from object name
        string cloneName = obj.name.Replace("(Clone)", string.Empty);
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == cloneName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled " + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        return poolType switch
        {
            PoolType.ParticleSystem => particleSystemEmpty,
            PoolType.Projectiles => projectilesEmpty,
            PoolType.Brawlers => brawlersEmpty,
            PoolType.Shooters => shootersEmpty,
            PoolType.Rollers => rollersEmpty,
            PoolType.Audio => audioSourceEmpty,
            PoolType.None => null,
            _ => null,
        };
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new();
}
