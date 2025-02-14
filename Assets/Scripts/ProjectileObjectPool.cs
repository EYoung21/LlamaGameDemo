using UnityEngine;
using System.Collections.Generic;

public class ProjectileObjectPool : MonoBehaviour
{
    public GameObject projectilePrefab;
    static GameObject[] pooledObjects = new GameObject[0];
    static ProjectileObjectPool _instance;
    static string pathToPrefab = "SpitProjectile";

    static int totalObjects = 0;
    static int freeObjectsInPool = 0;
    static int searchIndex = 0;

    private void Start()
    {
        IncreasePool(300);
    }

    private static void IncreasePool(int poolIncrease = 50)
    {
        if(_instance == null)
        {
            GameObject poolObject = new GameObject();
            poolObject.name = "Object Pool (instance)";
            _instance = poolObject.AddComponent<ProjectileObjectPool>();
            _instance.projectilePrefab = Resources.Load<GameObject>(pathToPrefab);
        }
        GameObject[] newPool = new GameObject[pooledObjects.Length + poolIncrease];
        for (int i = 0; i < pooledObjects.Length; i++)
        {
            newPool[i] = pooledObjects[i];
        }
        for (int i = pooledObjects.Length; i < pooledObjects.Length + poolIncrease; i++)
        {
            GameObject newObject = Instantiate<GameObject>(_instance.projectilePrefab, Vector3.zero, Quaternion.identity);
            newObject.SetActive(false);
            newPool[i] = newObject;
        }
        searchIndex = pooledObjects.Length;
        totalObjects += poolIncrease;
        freeObjectsInPool += poolIncrease;
        pooledObjects = newPool;
        Debug.Log("Increase Projectile Object Pool to: " + totalObjects);
    }

    public static GameObject RequestProjectile()
    {
        if(freeObjectsInPool == 0)
        {
            IncreasePool();
        }
        bool found = false;
        int i = searchIndex;
        while (!found)
        {
            if (pooledObjects[i].activeInHierarchy)
            {
                i++;
                i %= pooledObjects.Length;
            }
            else
                found = true;
        }
        freeObjectsInPool--;
        GameObject projectile = pooledObjects[i];
        searchIndex = i;
        projectile.SetActive(true);
        return projectile;
    }

    public static void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        freeObjectsInPool++;
    }
}
