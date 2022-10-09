using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// JSON 데이터를 받아오기 위한 serializable 클래스들
namespace Data
{
    [Serializable]
    public class Stat
    {
        public int level;
        public int maxHp;
        public int attack;
        public int totalExp;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach(Stat stat in stats)
            {
                dict.Add(stat.level, stat);
            }
            return dict;
        }
    }
}
