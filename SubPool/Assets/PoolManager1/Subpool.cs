using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理相同类型的对象
/// </summary>
public class Subpool
{
    List<GameObject> pool = new List<GameObject>();
    //要创建的游戏游戏对象预设体
    private GameObject prefab;

    //创建的预设体名字
    public Subpool(GameObject obj)
    {
        prefab = obj;
    }
    //返回预设体的名字,定义的预设体的名字与对象池一致,方便池子管理类找对应的池子
    public string name
    {
        get
        {
            return prefab.name;
        }
    }

    /// <summary>
    /// 从池子中拿对象
    /// </summary>
    /// <returns>The pool spawn.</returns>
    public GameObject SubPoolSpawn(Transform parent, Vector3 position, Quaternion quaternion)
    {
        GameObject obj = null;
        //遍历对象池中是否有可以使用的对象
        //有,就激活拿出来使用
        foreach (GameObject item in pool)
        {
            if (item.activeSelf == false)
            {
                obj = item;
                break;
            }
        }
        if (obj == null)
        {
            obj = GameObject.Instantiate(prefab) as GameObject;
            pool.Add(obj);
        }
        obj.transform.position = position;
        obj.transform.rotation = quaternion;
        obj.transform.SetParent(parent);
        obj.SetActive(true);
        //通过子类实例化接口对象,子类的脚本组件继承并实现了接口中的方法
        //control里面存的是该子类实现的方法,如果要生成一些特效,或者其他游戏行为,那么就可以继承IControl,通过它来进行调用


        IControl control = obj.GetComponent<IControl>();
        if (control != null)
        {
            control.Spawn();
        }

        return obj;
    }

    /// <summary>
    /// 回收游戏对象
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SubPoolUnSpawn(GameObject obj)
    {
        IControl control = obj.GetComponent<IControl>();
        if (control != null)
        {
            control.UnSpawn();
        }
        obj.SetActive(false);
    }

    /// <summary>
    /// 回收所有的游戏对象
    /// </summary>
    public void SubPoolUnSpawnAll()
    {
        //回收用于处于激活状态的游戏对象
        foreach (GameObject item in pool)
        {
            if (item.activeSelf)
            {
                SubPoolUnSpawn(item);
            }
        }
    }

    /// <summary>
    /// 检查某个游戏对象是否在对象池中
    /// </summary>
    /// <returns><c>true</c>, if pool contains was subed, <c>false</c> otherwise.</returns>
    /// <param name="obj">Object.</param>
    public bool SubPoolContains(GameObject obj)
    {
        return pool.Contains(obj);
    }

}