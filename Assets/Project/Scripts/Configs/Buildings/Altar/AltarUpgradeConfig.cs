using UnityEngine;
namespace Configs
{
    public class AltarUpgradeConfig
    {
        public readonly int CurrentUpgradeLevel;
        public readonly int UpgradeTime;
        public AltarUpgradeConfig(
            int currentUpgradeLevel,
            int upgradeTime)
        {
            CurrentUpgradeLevel = currentUpgradeLevel;
            UpgradeTime = upgradeTime;
        }
    }
}

