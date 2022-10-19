using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.Stat> StatDict { get; private set; } =
        new Dictionary<int, Data.Stat>();

    public void Init()
    {
        // "StatData.json" 파일을 읽어와서 저장한다.
        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    // 파일명을 받아서 json 파일을 읽어와서 Data.StatData 에 데이터를 넣어서 반환한다.
    // 이 때 Data.StatData 클래스는 public 이어야 하며 [Serializable] 필드가 있어야 한다.
    Loader LoadJson<Loader, Key, Value>(string path)
        where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
