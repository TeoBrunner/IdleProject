using UnityEngine;

namespace Configs
{
    public class HouseUpgradeConfig : BaseUpgradeConfig
    {
        public HouseUpgradeConfig(
            int currentUpgradeLevel, 
            int altarLevelRequired,
            int experienceLevelRequired, 
            int maxWorkers, 
            int upgradeGoldCost, 
            int upgradeWoodCost, 
            int upgradeStoneCost, 
            int upgradeShardsCost, 
            int upgradeTime) : base(
                currentUpgradeLevel, 
                altarLevelRequired, 
                experienceLevelRequired, 
                maxWorkers, 
                upgradeGoldCost, 
                upgradeWoodCost, 
                upgradeStoneCost, 
                upgradeShardsCost, 
                upgradeTime)
        {
        }
    }
}

