namespace Events
{
    public class AltarFlameChangedEvent
    {
        public readonly float CurrentFlame;
        public readonly int RequiredFlame;
        public readonly int Level;

        public AltarFlameChangedEvent(float current, int required, int level)
        {
            CurrentFlame = current;
            RequiredFlame = required;
            Level = level;
        }
    }
}