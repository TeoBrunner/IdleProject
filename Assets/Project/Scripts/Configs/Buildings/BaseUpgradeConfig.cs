using UnityEngine;

namespace Configs
{
    public abstract class BaseUpgradeConfig
    {
        public readonly int CurrentUpgradeLevel;
        public readonly int AltarLevelRequired;
        public readonly int MaxWorkers;
        public readonly int UpgradeGoldCost;
        public readonly int UpgradeWoodCost;
        public readonly int UpgradeStoneCost;
        public readonly int UpgradeShardsCost;

        public BaseUpgradeConfig(
            int currentUpgradeLevel,
            int altarLevelRequired,
            int maxWorkers,
            int upgradeGoldCost,
            int upgradeWoodCost,
            int upgradeStoneCost,
            int upgradeShardsCost)
        {
            CurrentUpgradeLevel = currentUpgradeLevel;
            AltarLevelRequired = altarLevelRequired;
            MaxWorkers = maxWorkers;
            UpgradeGoldCost = upgradeGoldCost;
            UpgradeWoodCost = upgradeWoodCost;
            UpgradeStoneCost = upgradeStoneCost;
            UpgradeShardsCost = upgradeShardsCost;
        }
    }

}

