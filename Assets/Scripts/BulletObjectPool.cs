using UnityEngine;
using System.Collections;
using UnityEngine.Pool;

public class BulletObjectPool : MonoBehaviour
{
    public GameObject projectilePrefab;
    static BulletObjectPool _instance;
    public static BulletObjectPool Instance {
        get
        {
            if (_instance == null)
            {
                GameObject poolObject = new GameObject();
                poolObject.name = "Object Pool (instance)";
                _instance = poolObject.AddComponent<BulletObjectPool>();
                _instance.projectilePrefab = Resources.Load<GameObject>(PATH_TO_PREFAB);
                _instance.Init();
            }
            return _instance;
        }
    }

    const string PATH_TO_PREFAB = "SpitProjectile";

    private ObjectPool<GameObject> bulletPool;

    void Init()
    {
        bulletPool = new ObjectPool<GameObject>(CreateBullet, OnGetBulletFromPool, OnReleaseBulletToPool, DestroyBullet, true, 500, 10000);
    }

    private GameObject CreateBullet()
    {
        GameObject newObject = Instantiate<GameObject>(Instance.projectilePrefab, Vector3.zero, Quaternion.identity);
        return newObject;
    }

    private void DestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }

    private void OnGetBulletFromPool(GameObject bullet)
    {
        bullet.SetActive(true);
    }

    private void OnReleaseBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    private void Start()
    {
        if (_instance != this)
            _instance = null;
    }

    public static GameObject RequestBullet()
    {
        return Instance.bulletPool.Get();
    }

    public static void ReturnBullet(GameObject bullet)
    {
        Instance.bulletPool.Release(bullet);
    }
}
