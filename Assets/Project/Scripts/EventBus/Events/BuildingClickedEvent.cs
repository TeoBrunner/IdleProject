namespace Events
{
    public class BuildingClickedEvent
    {
        public readonly Building Building;
        public BuildingClickedEvent(Building building) => Building = building;
    }
}

