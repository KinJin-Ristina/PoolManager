using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 缓存类型
/// </summary>
public enum BufferPoolType
{
    None,
    BULLET,
}

public class BufferPoolManager : MonoSingleton<BufferPoolManager>
{
    /// <summary>
    /// 存放所有池子的字典
    /// </summary>
    Dictionary<string, BufferPool> pools = new Dictionary<string, BufferPool>();

    /// <summary>
    /// 从池子取东西
    /// </summary>
    /// <param name="obj">实例化的物体</param>
    /// <param name="position">位置</param>
    /// <param name="quaternion">旋转</param>
    /// <param name="poolType">类型</param>
    public GameObject Spawn(GameObject obj, Vector3 position, Quaternion quaternion, BufferPoolType poolType)
    {
        BufferPool pool = null;
        //没有对应的池子，先生成池子
        if (!pools.ContainsKey(obj.name))
        {
            pool = new BufferPool(obj, poolType);
            pools.Add(pool.PoolName, pool);
        }
        pool = pools[obj.name];
        return pool.Spawn(position, quaternion);
    }

    /// <summary>
    /// 放入缓存池
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="poolType">需要的池子类型</param>
    public void UnSpawn(GameObject obj, BufferPoolType poolType)
    {
        foreach (BufferPool pool in pools.Values)
        {
            if (pool.PoolType == poolType)
            {
                pool.UnSpawn(obj);
                return;
            }
        }
    }
}
