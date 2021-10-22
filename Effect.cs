namespace MMRPGSkillSystem
{
    public class Effect
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PlayerSkills.Skill Skill { get; set; }
        public int Level { get; set; }
    }    
}
