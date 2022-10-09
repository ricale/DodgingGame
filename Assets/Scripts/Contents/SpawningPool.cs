using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;

    int _reserveCount = 0;

    [SerializeField]
    int _keepMonsterCount = 0;

    [SerializeField]
    List<Vector3> _spawnPos = new List<Vector3>();

    [SerializeField]
    float _spwanRadius = 15.0f;

    [SerializeField]
    float _spawnTime = 25.0f;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    void Start()
    {
        _spawnPos.Add(new Vector3(10.0f, 0, 10.0f));
        _spawnPos.Add(new Vector3(10.0f, 0, -10.0f));
        _spawnPos.Add(new Vector3(-10.0f, 0, 10.0f));
        _spawnPos.Add(new Vector3(-10.0f, 0, -10.0f));

        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        // 예약된 생성 숫자 + 이미 생성된 숫자 가 지정된 숫자보다 작으면 추가로 몬스터 생성
        while(_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount += 1;
        yield return new WaitForSeconds(Random.Range(1.0f, _spawnTime));
        GameObject go = Managers.Game.Spawn(Define.WorldObject.Monster, "Mummy");
        NavMeshAgent nma = go.GetOrAddComponent<NavMeshAgent>();

        Vector3 randPos;

        // TODO
        // 맵에 장애물이 생길 경우, 장애물과 겹치게 생성되면 안 됨
        // 맵에 장애물이 생길 경우, 플레이어에게 갈 길이 없는 곳에 생성되면 안 됨


        Vector3 randDir = Random.insideUnitSphere
                * Random.Range(0, _spwanRadius);
        // 평면 상에서 랜덤을 원하므로 y 는 0으로 지정
        // 그냥 y 를 0으로 하는게 맞는 건가? 이러면 제대로 된 랜덤이 아닐 것 같은데
        randDir.y = 0;
        int randIdx = Random.Range(0, _spawnPos.Count - 1);
        randPos = _spawnPos[randIdx] + randDir;

        NavMeshPath path = new NavMeshPath();

        go.transform.position = randPos;
        _reserveCount -= 1;
    }
}
