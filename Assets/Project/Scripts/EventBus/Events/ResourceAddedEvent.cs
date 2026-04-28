namespace Events
{
    public class ResourceAddedEvent
    {
        public readonly ResourceType ResourceType;
        public readonly int Amount;
        public ResourceAddedEvent(ResourceType type, int amount)
        {
            ResourceType = type;
            Amount = amount;
        }
    }
}
