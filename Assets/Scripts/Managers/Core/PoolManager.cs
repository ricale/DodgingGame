using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        // GameObject 를 count 개 만큼 미리 만들어 놓는다.
        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        // Original 을 복사해서 Poolable 컴포넌트를 붙인 뒤 반환
        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        // poolable 객체를 아래처럼 초기화
        //  1. 부모를 Root 으로 지정
        //  2. 게임 상에서 보이지 않도록 비활성화
        // 한 뒤, pool 스택에 저장
        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        // poolable 객체를 사용하기 위해 빼간다.
        public Poolable Pop(Transform parent)
        {
            Poolable poolable;
            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            // TODO
            // parent 값이 없을 경우 현재 씬을 부모로 지정
            //if(parent == null)
            //    poolable.transform.parent = Managers.Scene.CurrentScene.transform;
            //else
            poolable.transform.parent = parent;

            poolable.IsUsing = true;

            return poolable;
        }
    }

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    public void Init()
    {
        if(_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    // original 의 이름을 키값으로 original 의 pool 을 _pool 에 저장
    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    // _pool 에서 관리되는 객체라면 _pool 에 저장(반환), 아니라면 삭제
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if(_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }
        _pool[name].Push(poolable);
    }

    // 필요한 유형의 객체를 pop, 그런데 original 은 어떻게 받는 거지? 확인 필요
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);
        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if(_pool.ContainsKey(name) == false)
        {
            return null;
        }
        return _pool[name].Original;
    }

    public void Clear()
    {
        foreach(Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);
        }
        _pool.Clear();
    }
}
