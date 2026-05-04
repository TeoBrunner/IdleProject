namespace Configs
{
    public abstract class BaseWorkersConfig
    {
        public readonly int CurrentUpgradeLevel;
        public readonly int MaxWorkers;
        public readonly float WorkerProduction;
        public readonly float WorkerProductionInterval;

        public BaseWorkersConfig(
            int currentUpgradeLevel,
            int maxWorkers,
            float workerProduction,
            float workerProductionInterval)
        {
            CurrentUpgradeLevel = currentUpgradeLevel;
            MaxWorkers = maxWorkers;
            WorkerProduction = workerProduction;
            WorkerProductionInterval = workerProductionInterval;
        }
    }
}