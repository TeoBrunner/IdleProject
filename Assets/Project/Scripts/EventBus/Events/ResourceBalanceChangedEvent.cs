namespace Events
{
    public class ResourceBalanceChangedEvent
    {
        public readonly ResourceType ResourceType;
        public readonly int Amount;
        public ResourceBalanceChangedEvent(ResourceType type, int amount)
        {
            ResourceType = type;
            Amount = amount;
        }
    }
}
