namespace Events
{
    public class ResourceBalanceChangedEvent
    {
        public readonly ResourceType ResourceType;
        public readonly float Amount;
        public ResourceBalanceChangedEvent(ResourceType type, float amount)
        {
            ResourceType = type;
            Amount = amount;
        }
    }
}
