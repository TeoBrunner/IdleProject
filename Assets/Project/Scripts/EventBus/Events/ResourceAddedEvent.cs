using static Unity.VisualScripting.Member;

namespace Events
{
    public class ResourceAddedEvent
    {
        public readonly ResourceType ResourceType;
        public readonly float Amount;
        public readonly object Source;
        public ResourceAddedEvent(ResourceType resourceType, float amount, object source = null)
        {
            ResourceType = resourceType;
            Amount = amount;
            Source = source;
        }
    }
}
