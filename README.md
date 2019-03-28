# PoolManagerDemo
A PoolManager Practice

对象池类:

    //单例
    private static PoolManager instance;

    private PoolManager ()
    {

    }

    public static PoolManager Instance {
        get {
            if (instance == null) {
                instance = new PoolManager ();
            }
            return instance;
        }
    }

    //存储各类型的对象池的集合
    Dictionary<string,Subpool> poolDic = new Dictionary<string, Subpool> ();

    /// <summary>
    /// 添加对象池的方法
    /// </summary>
    /// <param name="name">Name.</param>
    void Register (string name)
    {
        GameObject obj = Resources.Load (name)as GameObject;
        Subpool subpool = new Subpool (obj);
        poolDic.Add (name, subpool);
    }

    /// <summary>
    /// 获取对象池中游戏对象
    /// </summary>
    /// <param name="name">Name.</param>
    public GameObject Spawn (string name)
    {
        if (!poolDic.ContainsKey (name)) {
            Register (name);
        }
        Subpool subpool = poolDic [name];
        return subpool.SubPoolSpawn ();
    }

    /// <summary>
    /// 回收游戏对象
    /// </summary>
    /// <param name="obj">Object.</param>
    public void UnSpawn (GameObject obj)
    {
        foreach (Subpool item in poolDic.Values) {
            if (item.SubPoolContains (obj)) {
                item.SubPoolUnSpawn (obj);
                break;
            }
        }
    }

    /// <summary>
    /// 清除游戏对象
    /// </summary>
    public void ClearPool ()
    {
        poolDic.Clear ();
    }
}

/// <summary>
/// 管理相同类型的对象
/// </summary>
public class Subpool
{
    List<GameObject> pool = new List<GameObject> ();
    //要创建的游戏游戏对象预设体
    private GameObject prefab;

    //创建的预设体名字
    public Subpool (GameObject obj)
    {
        prefab = obj;
    }
    //返回预设体的名字,定义的预设体的名字与对象池一致,方便池子管理类找对应的池子
    public string name {
        get {
            return prefab.name;
        }
    }

    /// <summary>
    /// 从池子中拿对象
    /// </summary>
    /// <returns>The pool spawn.</returns>
    public GameObject SubPoolSpawn ()
    {
        GameObject obj = null;
        //遍历对象池中是否有可以使用的对象
        //有,就激活拿出来使用
        foreach (GameObject item in pool) {
            if (item.activeSelf == false) {
                obj = item;
                break;
            }
        }
        if (obj == null) {
            obj = GameObject.Instantiate (prefab)as GameObject;
            pool.Add (obj);
        }
        obj.SetActive (true);

        //通过子类实例化接口对象,子类的脚本组件继承并实现了接口中的方法
        //control里面存的是该子类实现的方法,如果要生成一些特效,或者其他游戏行为,那么就可以继承IControl,通过它来进行调用


        IControl control = obj.GetComponent <IControl> ();
        if (control != null) {
            control.Spawn ();
        }

        return obj;
    }

    /// <summary>
    /// 回收游戏对象
    /// </summary>
    /// <param name="obj">Object.</param>
    public void SubPoolUnSpawn (GameObject obj)
    {
        IControl control = obj.GetComponent <IControl> ();
        if (control != null) {
            control.UnSpawn ();
        }
        obj.SetActive (false);
    }

    /// <summary>
    /// 回收所有的游戏对象
    /// </summary>
    public void SubPoolUnSpawnAll ()
    {
        //回收用于处于激活状态的游戏对象
        foreach (GameObject item in pool) {
            if (item.activeSelf) {
                SubPoolUnSpawn (item);
            }
        }
    }

    /// <summary>
    /// 检查某个游戏对象是否在对象池中
    /// </summary>
    /// <returns><c>true</c>, if pool contains was subed, <c>false</c> otherwise.</returns>
    /// <param name="obj">Object.</param>
    public bool SubPoolContains (GameObject obj)
    {
        return pool.Contains (obj);
    }

}
 
public interface IControl
{

    //从对象池中取对象的方法
    void Spawn () ;


    //销毁对象到对象池的方法
    void UnSpawn () ;

}

/// <summary>
/// 生成子弹,挂载在Manager上
/// </summary>
public class CreatBulletScript : MonoBehaviour
{
    public static CreatBulletScript Instance;

    int count = 0;

    void Awake ()
    {
        Instance = this;
    }

    void SpawnBullet ()
    {
        GameObject temp = PoolManager.Instance.Spawn ("Bullet");
        temp.transform.SetParent (transform);
        temp.transform.localPosition = Vector3.zero;
        count++;
    }

    /// <summary>
    /// 可以给外部调用
    /// </summary>
    /// <param name="obj">Object.</param>
    public void UnSpawnBullet (GameObject obj)
    {
        PoolManager.Instance.UnSpawn (obj);
        count--;
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown (0)) {
            SpawnBullet ();
        }
    }
}

/// <summary>
/// 挂载在Bullet上
/// </summary>
public class Bullet : MonoBehaviour
{

    float timer = 0;
    /// <summary>
    /// 时间间隔
    /// </summary>
    float timeInterval = 2;

    void Update ()
    {
        timer += Time.deltaTime;
        transform.localPosition += new Vector3 (0, 0, 1);
        if (timer >= timeInterval) {
            CreatBulletScript.Instance.UnSpawnBullet (gameObject);
            timer = 0;
        }
    }
}

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
