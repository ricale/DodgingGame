﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    // 월드에 있는 오브젝트의 유형
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }
    // 플레이어/몬스터 상태
    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill
    }
    // 레이어
    public enum Layer
    {
        Ground = 8
    }
}
