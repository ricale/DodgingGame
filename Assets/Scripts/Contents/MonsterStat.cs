using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat
{
    void Start()
    {
        Level = 1;
        Hp = 50;
        MaxHp = 100;
        Attack = 1;
        Defense = 0;

        CollisionDamage = 1.0f;
        MoveSpeed = 1.6f;
    }

    protected override void OnDead(Stat attacker)
    {
        gameObject.GetComponent<MonsterController>().Dead();
    }
}
