namespace Events
{
    public class BuildingExaminedEvent
    {
        public readonly Building Building;
        public BuildingExaminedEvent(Building building) => Building = building;
    }
}

