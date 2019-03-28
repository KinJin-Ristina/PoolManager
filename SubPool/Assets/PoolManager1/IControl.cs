using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControl
{

    //从对象池中取对象的方法
    void Spawn();


    //销毁对象到对象池的方法
    void UnSpawn();

}