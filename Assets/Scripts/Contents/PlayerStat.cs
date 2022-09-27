using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;

            // TODO
            // - 레벨업 처리
        }
    }

    public int Gold { get { return _gold; } set { _gold = value; } }

    void Start()
    {
        Level = 1;
        Exp = 0;
        Defense = 5;
        MoveSpeed = 5.0f;
        Gold = 0;
    }

    public void SetStat(int level)
    {
        // TODO
        // - 데이터 파일에서 스탯 가져오기
    }

    protected override void OnDead(Stat attacker)
    {
        // TODO
        // - 죽었을 때 처리
    }
}
