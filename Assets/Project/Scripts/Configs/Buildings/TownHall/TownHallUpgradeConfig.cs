using UnityEngine;

namespace Configs
{
	public class TownHallUpgradeConfig : BaseUpgradeConfig
	{
		public TownHallUpgradeConfig(
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