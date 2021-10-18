using System.Collections.Generic;

namespace MMRPGSkillSystem
{
    public class ExpTable
    {
        public static List<MonsterExp> MonsterExpList { get; set; }

        public static void InitMonsterExpList()
        {
            MonsterExpList = new List<MonsterExp>();
            MonsterExpList.Add(new MonsterExp("Boar", 100));
            MonsterExpList.Add(new MonsterExp("Player", 100));
            MonsterExpList.Add(new MonsterExp("Neck", 100));
            MonsterExpList.Add(new MonsterExp("Neck", 100));
            MonsterExpList.Add(new MonsterExp("Neck", 100));
            MonsterExpList.Add(new MonsterExp("Greyling", 100));
            MonsterExpList.Add(new MonsterExp("RRRN_SS_CavaleiroNegro", 100));
            MonsterExpList.Add(new MonsterExp("RRRN_popvillagesHumanM10", 100));
            MonsterExpList.Add(new MonsterExp("RRRN_popvillagesHumanM02", 100));
            MonsterExpList.Add(new MonsterExp("RRRN_popvillagesHumanM01", 100));
            MonsterExpList.Add(new MonsterExp("RRRN_popvillagesHumanF01", 100));
            MonsterExpList.Add(new MonsterExp("RRRN_popvillagesElf01", 100));
            MonsterExpList.Add(new MonsterExp("RRRN_popvillagesDwarf01", 100));
            MonsterExpList.Add(new MonsterExp("RRRN_DHNPC05", 100));
            MonsterExpList.Add(new MonsterExp("RRRM_BrownWolf", 100));
            MonsterExpList.Add(new MonsterExp("RRRM_BlackWolf", 100));
            MonsterExpList.Add(new MonsterExp("RRRM_AshTrollFogo", 100));
            MonsterExpList.Add(new MonsterExp("RRRN_AshSurtr", 100));
            MonsterExpList.Add(new MonsterExp("RRRN_AshGiganteFogo", 100));
            MonsterExpList.Add(new MonsterExp("RRRM_AshDragaoFogo", 100));
            MonsterExpList.Add(new MonsterExp("RRRM_AshDracoFogo", 100));
            MonsterExpList.Add(new MonsterExp("RRRM_AshBlob", 100));
            MonsterExpList.Add(new MonsterExp("RRR_Hostile_T6", 100));
            MonsterExpList.Add(new MonsterExp("RRR_Hostile_T5", 100));
            MonsterExpList.Add(new MonsterExp("RRR_Hostile_T4", 100));
            MonsterExpList.Add(new MonsterExp("RRR_Hostile_T3", 100));
            MonsterExpList.Add(new MonsterExp("RRR_Hostile_T2", 100));
            MonsterExpList.Add(new MonsterExp("RRR_Hostile_T1", 100));
            MonsterExpList.Add(new MonsterExp("RRR_NPC", 100));
            MonsterExpList.Add(new MonsterExp("RRR_TrollTosser", 100));
            MonsterExpList.Add(new MonsterExp("RRR_GhostVengeful", 100));
            MonsterExpList.Add(new MonsterExp("RRR_GDThornweaver", 100));
            MonsterExpList.Add(new MonsterExp("RRR_Grig", 100));
            MonsterExpList.Add(new MonsterExp("Player", 100));
            MonsterExpList.Add(new MonsterExp("gd_king", 100));
            MonsterExpList.Add(new MonsterExp("BlobTar", 100));
            MonsterExpList.Add(new MonsterExp("Blob", 100));
            MonsterExpList.Add(new MonsterExp("Boar", 100));
            MonsterExpList.Add(new MonsterExp("Lox", 100));
            MonsterExpList.Add(new MonsterExp("Wolf", 100));
            MonsterExpList.Add(new MonsterExp("Lox_Calf", 100));
            MonsterExpList.Add(new MonsterExp("Wolf_cub", 100));
            MonsterExpList.Add(new MonsterExp("Boar_piggy", 100));
            MonsterExpList.Add(new MonsterExp("BlobElite", 100));
            MonsterExpList.Add(new MonsterExp("Skeleton_Poison", 100));
            MonsterExpList.Add(new MonsterExp("Draugr", 100));
            MonsterExpList.Add(new MonsterExp("Leech", 100));
            MonsterExpList.Add(new MonsterExp("Leech_cave", 100));
            MonsterExpList.Add(new MonsterExp("Serpent", 100));
            MonsterExpList.Add(new MonsterExp("Ghost", 100));
            MonsterExpList.Add(new MonsterExp("Neck", 100));
            MonsterExpList.Add(new MonsterExp("Draugr_Elite", 100));
            MonsterExpList.Add(new MonsterExp("Greydwarf_Shaman", 100));
            MonsterExpList.Add(new MonsterExp("TentaRoot", 100));
            MonsterExpList.Add(new MonsterExp("Greydwarf", 100));
            MonsterExpList.Add(new MonsterExp("Skeleton", 100));
            MonsterExpList.Add(new MonsterExp("TrainingDummy", 100));
            MonsterExpList.Add(new MonsterExp("Greydwarf_Elite", 100));
            MonsterExpList.Add(new MonsterExp("Fenring", 100));
            MonsterExpList.Add(new MonsterExp("Hatchling", 100));
            MonsterExpList.Add(new MonsterExp("Deathsquito", 100));
            MonsterExpList.Add(new MonsterExp("Dragon", 100));
            MonsterExpList.Add(new MonsterExp("Wraith", 100));
            MonsterExpList.Add(new MonsterExp("GoblinBrute", 100));
            MonsterExpList.Add(new MonsterExp("GoblinKing", 100));
            MonsterExpList.Add(new MonsterExp("StoneGolem", 100));
            MonsterExpList.Add(new MonsterExp("Troll", 100));
            MonsterExpList.Add(new MonsterExp("Eikthyr", 100));
            MonsterExpList.Add(new MonsterExp("Goblin", 100));
            MonsterExpList.Add(new MonsterExp("GoblinShaman", 100));
            MonsterExpList.Add(new MonsterExp("Surtling", 100));
            MonsterExpList.Add(new MonsterExp("Bonemass", 100));
            MonsterExpList.Add(new MonsterExp("Skeleton_NoArcher", 100));
            MonsterExpList.Add(new MonsterExp("GoblinArcher", 100));
            MonsterExpList.Add(new MonsterExp("Greyling", 100));
            MonsterExpList.Add(new MonsterExp("Draugr_Ranged", 100));
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
