using UnityEngine;

namespace Configs
{
    public class TownHallExperienceConfig
    {
        public readonly int ExpLevel;
        public readonly int ExpPointsRequired;
        public readonly int CumulativeExpPointsRequired;

        public TownHallExperienceConfig(
            int expLevel, 
            int expPointsRequired, 
            int cumulativeExpPointsRequired)
        {
            ExpLevel = expLevel;
            ExpPointsRequired = expPointsRequired;
            CumulativeExpPointsRequired = cumulativeExpPointsRequired;
        }
    }
}