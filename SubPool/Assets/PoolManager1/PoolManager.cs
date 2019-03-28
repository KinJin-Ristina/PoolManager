using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对象池类
/// </summary>
public class PoolManager
{
    //单例
    private static PoolManager instance;

    private PoolManager()
    {

    }

    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PoolManager();
            }
            return instance;
        }
    }

    //存储各类型的对象池的集合
    Dictionary<string, Subpool> poolDic = new Dictionary<string, Subpool>();

    /// <summary>
    /// 添加对象池的方法
    /// </summary>
    /// <param name="name">Name.</param>
    void Register(GameObject _obj)
    {
        GameObject obj = _obj;
        Subpool subpool = new Subpool(obj);
        poolDic.Add(obj.name, subpool);
    }

    /// <summary>
    /// 获取对象池中游戏对象
    /// </summary>
    /// <param name="name">Name.</param>
    public GameObject Spawn(GameObject obj, Transform parent, Vector3 position, Quaternion quaternion)
    {
        if (!poolDic.ContainsKey(obj.name))
        {
            Register(obj);
        }
        Subpool subpool = poolDic[obj.name];
        return subpool.SubPoolSpawn(parent, position, quaternion);
    }

    /// <summary>
    /// 回收游戏对象
    /// </summary>
    /// <param name="obj">Object.</param>
    public void UnSpawn(GameObject obj)
    {
        foreach (Subpool item in poolDic.Values)
        {
            if (item.SubPoolContains(obj))
            {
                item.SubPoolUnSpawn(obj);
                break;
            }
        }
    }

    /// <summary>
    /// 清除游戏对象
    /// </summary>
    public void ClearPool()
    {
        poolDic.Clear();
    }
}