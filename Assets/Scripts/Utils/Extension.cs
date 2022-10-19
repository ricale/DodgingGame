using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    // 이제 GameObject 에서 이 메서드를 쓸 수 있다
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
