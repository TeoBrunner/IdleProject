using UnityEngine;
namespace Configs
{
    public class TownHallWorkersConfig : BaseWorkersConfig
    {
        public TownHallWorkersConfig(
            int currentUpgradeLevel, 
            int maxWorkers, 
            float workerProduction, 
            float workerProductionInterval) : base(
                currentUpgradeLevel, 
                maxWorkers, 
                workerProduction, 
                workerProductionInterval)
        {
        }
    }
}

