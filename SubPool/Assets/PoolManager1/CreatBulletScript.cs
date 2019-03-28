using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成子弹,挂载在Manager上
/// </summary>
public class CreatBulletScript : MonoBehaviour
{
    public static CreatBulletScript Instance;

    int count = 0;

    void Awake()
    {
        Instance = this;
    }

    void SpawnBullet()
    {
        GameObject temp = PoolManager.Instance.Spawn(Resources.Load("Bullet") as GameObject, transform, transform.position, transform.rotation);
        //temp.transform.SetParent(transform);
        //temp.transform.localPosition = Vector3.zero;

        //GameObject temp = BufferPoolManager.Instance.Spawn(Resources.Load("Bullet") as GameObject, transform.position, transform.rotation, BufferPoolType.BULLET);
        count++;
    }

    /// <summary>
    /// 可以给外部调用
    /// </summary>
    /// <param name="obj">Object.</param>
    public void UnSpawnBullet(GameObject obj)
    {
        PoolManager.Instance.UnSpawn(obj);
        //BufferPoolManager.Instance.UnSpawn(obj, BufferPoolType.BULLET);
        count--;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBullet();
        }
    }
}