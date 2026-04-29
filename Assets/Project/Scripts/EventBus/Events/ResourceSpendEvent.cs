namespace Events
{
    public class ResourceSpendEvent
    {
        public readonly ResourceType ResourceType;
        public readonly float Amount;
        public ResourceSpendEvent(ResourceType type, float amount)
        {
            ResourceType = type;
            Amount = amount;
        }
    }
}