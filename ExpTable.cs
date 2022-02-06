using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ValheimLevelSystem
{
    public class ExpTable
    {
        public static List<MonsterExp> MonsterExpList { get; set; }

        public static void InitMonsterExpList()
        {
            MonsterExpList = new List<MonsterExp>();

            ValheimLevelSystem.Tier1Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, ValheimLevelSystem.Tier1Exp.Value, 1)));        
            ValheimLevelSystem.Tier2Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, ValheimLevelSystem.Tier2Exp.Value, 2)));        
            ValheimLevelSystem.Tier3Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, ValheimLevelSystem.Tier3Exp.Value, 3)));        
            ValheimLevelSystem.Tier4Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, ValheimLevelSystem.Tier4Exp.Value, 4)));        
            ValheimLevelSystem.Tier5Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, ValheimLevelSystem.Tier5Exp.Value, 5)));        
            ValheimLevelSystem.Tier6Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, ValheimLevelSystem.Tier6Exp.Value, 6)));        
            ValheimLevelSystem.Tier7Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, ValheimLevelSystem.Tier7Exp.Value, 7)));        
            ValheimLevelSystem.Tier8Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, ValheimLevelSystem.Tier8Exp.Value, 8)));        
            ValheimLevelSystem.Tier9Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, ValheimLevelSystem.Tier9Exp.Value, 9)));        
            ValheimLevelSystem.Tier10Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, ValheimLevelSystem.Tier10Exp.Value, 10)));
        }
    }

    public class MonsterExp
    {
        public MonsterExp(string name, int exp = 100, int tier = 1)
        {
            Name = name;
            ExpAmount = exp;
            Tier = tier;
        }

        public string Name { get; set; }
        public int ExpAmount { get; set; }
        public int Tier { get; set; } 
    }
}
