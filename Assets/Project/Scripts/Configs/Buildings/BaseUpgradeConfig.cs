using UnityEngine;

namespace Configs
{
    public abstract class BaseUpgradeConfig
    {
        public readonly int CurrentUpgradeLevel;
        public readonly int AltarLevelRequired;
        public readonly int ExperienceLevelRequired;
        public readonly int MaxWorkers;
        public readonly int UpgradeGoldCost;
        public readonly int UpgradeWoodCost;
        public readonly int UpgradeStoneCost;
        public readonly int UpgradeShardsCost;
        public readonly int UpgradeTime;

        public BaseUpgradeConfig(
            int currentUpgradeLevel,
            int altarLevelRequired,
            int experienceLevelRequired,
            int maxWorkers,
            int upgradeGoldCost,
            int upgradeWoodCost,
            int upgradeStoneCost,
            int upgradeShardsCost,
            int upgradeTime)
        {
            CurrentUpgradeLevel = currentUpgradeLevel;
            AltarLevelRequired = altarLevelRequired;
            ExperienceLevelRequired = experienceLevelRequired;
            MaxWorkers = maxWorkers;
            UpgradeGoldCost = upgradeGoldCost;
            UpgradeWoodCost = upgradeWoodCost;
            UpgradeStoneCost = upgradeStoneCost;
            UpgradeShardsCost = upgradeShardsCost;
            UpgradeTime = upgradeTime;
        }
    }

}

