using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存物体继承的接口
/// </summary>
public interface IReuse
{
    /// <summary>
    /// 出池子
    /// </summary>
    void Spawn();

    /// <summary>
    /// 进池子(初始化参数)
    /// </summary>
    void UnSpawn();

}
