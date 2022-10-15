using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance
    {
        get
        {
            init();
            return s_instance;
        }
    }

    #region Contents
    GameManager _game = new GameManager();

    public static GameManager Game { get { return Instance._game; } }
    #endregion

    #region Core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();

    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

    static void init()
    {
        if (s_instance != null)
        {
            return;
        }

        GameObject go = GameObject.Find("@Managers");
        if (go == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }

        DontDestroyOnLoad(go);

        // 게임 상에 포함되기 위해 게임오브젝트에 컴포넌트로서 추가된 상태
        s_instance = go.GetComponent<Managers>();

        s_instance._data.Init();
        s_instance._pool.Init();
    }

    public static void Clear()
    {
        Pool.Clear();
    }

    void Start()
    {
        init();
    }

    void Update()
    {
        Input.OnUpdate();
    }
}
