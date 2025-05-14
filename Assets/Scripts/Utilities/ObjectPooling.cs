using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    // create new object pool for ever game object passed into it
    private GameObject _objectPoolParentHolder;

    private static GameObject _projectilesParent;
    private static GameObject _enemiesParent;
    private static GameObject _brawlersParent;
    private static GameObject _shootersParent;
    private static GameObject _rollersParent;

    private static GameObject _audioSourceParent;

    private int _projectileMaxCount;

    private int _enemyMaxCount;

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

        _enemyMaxCount = 15;
        _projectileMaxCount = 40;
    }

    private void SetupParent()
    {
        _objectPoolParentHolder = new GameObject("Pooled Objects");

        _projectilesParent = new GameObject("Projectile Objects");
        _projectilesParent.transform.SetParent(_objectPoolParentHolder.transform);

        _enemiesParent = new GameObject("Enemy Objects");
        _enemiesParent.transform.SetParent(_objectPoolParentHolder.transform);

        _brawlersParent = new GameObject("Brawler Objects");
        _brawlersParent.transform.SetParent(_enemiesParent.transform);

        _shootersParent = new GameObject("Shooter Objects");
        _shootersParent.transform.SetParent(_enemiesParent.transform);

        _rollersParent = new GameObject("Roller Objects");
        _rollersParent.transform.SetParent(_enemiesParent.transform);

        _audioSourceParent = new GameObject("Audio Source Objects");
        _audioSourceParent.transform.SetParent(_objectPoolParentHolder.transform);
    }

    public static PoolType PoolingType;

    public static List<PooledObjectInfo> ObjectPools = new();

    private void Update()
    {
        CullPool(_projectilesParent, _projectileMaxCount);

        CullPool(_brawlersParent, _enemyMaxCount);
        CullPool(_shootersParent, _enemyMaxCount);
        CullPool(_rollersParent, _enemyMaxCount);

        CullAudioPool(_audioSourceParent);
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
            PoolType.Projectiles => _projectilesParent,
            PoolType.Brawlers => _brawlersParent,
            PoolType.Shooters => _shootersParent,
            PoolType.Rollers => _rollersParent,
            PoolType.Audio => _audioSourceParent,
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
