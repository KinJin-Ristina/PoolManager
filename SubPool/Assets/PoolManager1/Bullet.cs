using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载在Bullet上
/// </summary>
public class Bullet : MonoBehaviour, IControl
{
    protected ParticleSystem[] pss;
    float timer = 0;
    /// <summary>
    /// 时间间隔
    /// </summary>
    float timeInterval = 2;
    void Awake()
    {
        pss = transform.GetComponentsInChildren<ParticleSystem>();
    }

    public void Spawn()
    {
        Debug.Log("Spawn");
        PlayEffect();
    }

    public void UnSpawn()
    {
        Debug.Log("UnSpawn");
        ResetEffect();
    }

    void Update()
    {
        timer += Time.deltaTime;
        transform.localPosition += new Vector3(0, 0, 1);
        if (timer >= timeInterval)
        {
            CreatBulletScript.Instance.UnSpawnBullet(gameObject);
            timer = 0;
        }
    }
    /// <summary>
    /// 播放特效
    /// </summary>
    void PlayEffect()
    {
        foreach (var item in pss)
        {
            item.Play();
        }
    }

    /// <summary>
    /// 结束特效
    /// </summary>
    void ResetEffect()
    {
        foreach (var item in pss)
        {
            item.Stop();
        }
    }
}