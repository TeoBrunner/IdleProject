namespace Events
{
    public class ResourceSpendEvent
    {
        public readonly ResourceType ResourceType;
        public readonly int Amount;
        public ResourceSpendEvent(ResourceType type, int amount)
        {
            ResourceType = type;
            Amount = amount;
        }
    }
}