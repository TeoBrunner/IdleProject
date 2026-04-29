namespace Events
{
    public class BuildingExperienceChangedEvent
    {
        public readonly Building Building;
        public readonly int Level;
        public readonly int CurrentExp;
        public readonly int RequiredExp;

        public BuildingExperienceChangedEvent(Building building, int level, int currentExp, int requiredExp)
        {
            Building = building;
            Level = level;
            CurrentExp = currentExp;
            RequiredExp = requiredExp;
        }
    }
}
