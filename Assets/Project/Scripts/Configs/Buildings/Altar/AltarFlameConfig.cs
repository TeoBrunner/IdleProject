using UnityEngine;
namespace Configs
{
    public class AltarFlameConfig
    {
        public readonly int CurrentUpgradeLevel;
        public readonly int FlamePointsRequired;
        public readonly int CumulativeFlamePointsRequired;
        public AltarFlameConfig(
            int currentUpgradeLevel,
            int flamePointsRequired,
            int cumulativeFlamePointsRequired)
        {
            CurrentUpgradeLevel = currentUpgradeLevel;
            FlamePointsRequired = flamePointsRequired;
            CumulativeFlamePointsRequired = cumulativeFlamePointsRequired;
        }
    }

}
