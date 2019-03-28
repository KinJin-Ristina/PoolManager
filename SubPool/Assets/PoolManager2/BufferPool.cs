using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存池模型
/// </summary>
public class BufferPool
{
    /// <summary>
    /// 池子类型
    /// </summary>
    private BufferPoolType poolType;

    /// <summary>
    /// 返回池子类型
    /// </summary>
    /// <value>The type of the pool.</value>
    public BufferPoolType PoolType
    {
        get
        {
            return poolType;
        }
    }

    /// <summary>
    /// 存放的对象
    /// </summary>
    private GameObject prefab;

    /// <summary>
    /// 返回预制体的名称
    /// </summary>
    /// <value>预制体的名字</value>
    public string PoolName
    {
        get
        {
            return prefab.name;
        }
    }

    /// <summary>
    /// 当前对象的容器
    /// </summary>
    Queue<GameObject> bufferPool = new Queue<GameObject>();

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="_prefab">存放的对象</param>
    /// <param name="_poolType">类型</param>
    public BufferPool(GameObject _prefab, BufferPoolType _poolType)
    {
        prefab = _prefab;
        poolType = _poolType;
    }

    /// <summary>
    /// 从池子取东西
    /// </summary>
    /// <param name="position">Position.</param>
    /// <param name="quaternion">Quaternion.</param>
    public GameObject Spawn(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = null;
        if (bufferPool.Count > 0)
        {
            obj = bufferPool.Dequeue();
        }
        else
        {
            obj = GameObject.Instantiate(prefab) as GameObject;
            //          bufferPool.Enqueue (obj);
        }
        obj.transform.position = position;
        obj.transform.rotation = quaternion;
        obj.SetActive(true);
        //出来后的处理
        obj.GetComponent<IReuse>().Spawn();
        return obj;
    }

    /// <summary>
    /// 进入缓存池
    /// </summary>
    /// <param name="obj">Object.</param>
    public void UnSpawn(GameObject obj)
    {
        obj.SetActive(false);
        bufferPool.Enqueue(obj);
        //进入缓存后的处理
        obj.GetComponent<IReuse>().UnSpawn();
    }
}
