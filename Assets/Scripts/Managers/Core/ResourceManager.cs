using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    // path 에 있는 prefab 을 로드한다.
    // 해당 객체가 poolable 이라면 pool 로부터 객체를 가져와 반환한다.
    // 해당 객체가 poolable 이 아니라면 prefab 을 복사한 뒤 반환한다.
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to load prefab: {path}");
            return null;
        }

        if(original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;
    }

    // 게임 오브젝트 삭제
    // poolable 한 오브젝트라면 Pool 에 다시 밀어넣는다.
    // 아니라면 진짜 삭제
    public void Destroy(GameObject go)
    {
        if(go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
