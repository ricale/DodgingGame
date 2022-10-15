using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    GameObject _player;
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    public GameObject GetPlayer() { return _player; }

    // 게임 오브젝트 생성
    public GameObject Spawn(
        Define.WorldObject type,
        string path,
        Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch(type)
        {
            // 몬스터라면 몬스터 셋에 넣어 놓고, spawn 이벤트 발생
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                if(OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
            // 플레이어라면 별 일 안 함
            case Define.WorldObject.Player:
                _player = go;
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        CharacterControllerBase bc = go.GetComponent<CharacterControllerBase>();
        if(bc == null)
        {
            return Define.WorldObject.Unknown;
        }
        return bc.WorldObjectType;
    }

    // 게임 오브젝트 삭제
    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch(type)
        {
            // 몬스터라면 몬스터 셋에서 지우고, spawn 이벤트 발생
            case Define.WorldObject.Monster:
                if(_monsters.Contains(go))
                {
                    _monsters.Remove(go);
                    if(OnSpawnEvent != null)
                    {
                        OnSpawnEvent.Invoke(-1);
                    }
                }
                break;
            // 플레이어라면 별 일 안 함
            case Define.WorldObject.Player:
                if(_player == go)
                {
                    _player = null;
                }
                break;
        }

        Managers.Resource.Destroy(go);
    }
}
