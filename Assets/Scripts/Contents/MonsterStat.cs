using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat
{
    void Start()
    {
        CollisionDamage = 1.0f;
        MoveSpeed = 1.6f;
    }
}
