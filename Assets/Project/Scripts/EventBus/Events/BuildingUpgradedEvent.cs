namespace Events
{
    public class BuildingUpgradedEvent
    {
        public readonly Building Building;
        public BuildingUpgradedEvent(Building building) => Building = building;
    }
}