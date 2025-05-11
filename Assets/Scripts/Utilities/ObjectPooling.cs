using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    // create new object pool for ever game object passed into it
    private GameObject objectPoolParentHolder;

    private static GameObject projectilesParent;
    private static GameObject enemiesParent;
    private static GameObject brawlersParent;
    private static GameObject shootersParent;
    private static GameObject rollersParent;

    private static GameObject audioSourceParent;

    private int projectileMaxCount;

    private int enemyMaxCount;

    public enum PoolType
    {
        Projectiles,

        Brawlers,
        Shooters,
        Rollers,
        Audio,
        None
    }

    private void Awake()
    {
        SetupParent();

        enemyMaxCount = 15;
        projectileMaxCount = 40;
    }

    private void SetupParent()
    {
        objectPoolParentHolder = new GameObject("Pooled Objects");

        projectilesParent = new GameObject("Projectile Objects");
        projectilesParent.transform.SetParent(objectPoolParentHolder.transform);

        enemiesParent = new GameObject("Enemy Objects");
        enemiesParent.transform.SetParent(objectPoolParentHolder.transform);

        brawlersParent = new GameObject("Brawler Objects");
        brawlersParent.transform.SetParent(enemiesParent.transform);

        shootersParent = new GameObject("Shooter Objects");
        shootersParent.transform.SetParent(enemiesParent.transform);

        rollersParent = new GameObject("Roller Objects");
        rollersParent.transform.SetParent(enemiesParent.transform);

        audioSourceParent = new GameObject("Audio Source Objects");
        audioSourceParent.transform.SetParent(objectPoolParentHolder.transform);
    }

    public static PoolType PoolingType;

    public static List<PooledObjectInfo> ObjectPools = new();

    private void Update()
    {
        CullPool(projectilesParent, projectileMaxCount);

        CullPool(brawlersParent, enemyMaxCount);
        CullPool(shootersParent, enemyMaxCount);
        CullPool(rollersParent, enemyMaxCount);

        CullAudioPool(audioSourceParent);
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
            Debug.LogWarning("Trying to release non-pooled object: " + obj.name);
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
            PoolType.Projectiles => projectilesParent,
            PoolType.Brawlers => brawlersParent,
            PoolType.Shooters => shootersParent,
            PoolType.Rollers => rollersParent,
            PoolType.Audio => audioSourceParent,
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
