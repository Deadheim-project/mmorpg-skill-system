using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MMRPGSkillSystem
{
    public class ExpTable
    {
        public static List<MonsterExp> MonsterExpList { get; set; }

        public static void InitMonsterExpList()
        {
            MonsterExpList = new List<MonsterExp>();

            MMRPGSkillSystem.Tier1Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, MMRPGSkillSystem.Tier1Exp.Value, 1)));        
            MMRPGSkillSystem.Tier2Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, MMRPGSkillSystem.Tier2Exp.Value, 2)));        
            MMRPGSkillSystem.Tier3Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, MMRPGSkillSystem.Tier3Exp.Value, 3)));        
            MMRPGSkillSystem.Tier4Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, MMRPGSkillSystem.Tier4Exp.Value, 4)));        
            MMRPGSkillSystem.Tier5Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, MMRPGSkillSystem.Tier5Exp.Value, 5)));        
            MMRPGSkillSystem.Tier6Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, MMRPGSkillSystem.Tier6Exp.Value, 6)));        
            MMRPGSkillSystem.Tier7Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, MMRPGSkillSystem.Tier7Exp.Value, 7)));        
            MMRPGSkillSystem.Tier8Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, MMRPGSkillSystem.Tier8Exp.Value, 8)));        
            MMRPGSkillSystem.Tier9Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, MMRPGSkillSystem.Tier9Exp.Value, 9)));        
            MMRPGSkillSystem.Tier10Creatures.Value.Split(',').ToList().ForEach(x => MonsterExpList.Add(new MonsterExp(x, MMRPGSkillSystem.Tier10Exp.Value, 10)));

            MonsterExpList.ForEach(x => Debug.LogError(x.Name + x.ExpAmount));
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
