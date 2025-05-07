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

    public enum PoolType
    {
        ParticleSystem,
        Projectiles,
        Enemies,
        None
    }

    private void Awake()
    {
        SetupEmpty();
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
    }

    public static PoolType PoolingType;

    public static List<PooledObjectInfo> ObjectPools = new();

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
            PoolType.Enemies => enemiesEmpty,
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
