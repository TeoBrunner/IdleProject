using UnityEngine;

namespace Configs
{
	public class TownHallUpgradeConfig : BaseUpgradeConfig
	{
		public TownHallUpgradeConfig(int currentUpgradeLevel, int altarLevelRequired, int maxWorkers, int upgradeGoldCost, int upgradeWoodCost, int upgradeStoneCost, int upgradeShardsCost) : base(currentUpgradeLevel, altarLevelRequired, maxWorkers, upgradeGoldCost, upgradeWoodCost, upgradeStoneCost, upgradeShardsCost)
		{
		}
    }
}