using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public T MakeWorldSpaceUI<T>(
        Transform parent = null,
        string name = null) where T : UI_Base
    {
        // 이름이 없으면 타입 T 의 이름 (i.e. UI_HpBar) 을 이름으로 지정
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // name 과 동일한 이름의  prefab 을 불러옴
        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");

        // 부모 지정
        if (parent != null)
            go.transform.SetParent(parent);

        // UI 를 월드스페이스 소속으로 설정하고, 메인 카메라도 붙여준다
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        // prefabs 에 T 컴포넌트를 붙여서 반환한.
        return go.GetOrAddComponent<T>();
    }
}
