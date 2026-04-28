using UnityEngine;

namespace Configs
{
    public class HouseWorkersConfig
    {
        public readonly int Workers;
        public readonly int WorkerGoldCost;
        public readonly int CumulativeWorkerGoldCost;
        public HouseWorkersConfig(
            int workers,
            int workerGoldCost,
            int cumulativeWorkerGoldCost)
        {
            Workers = workers;
            WorkerGoldCost = workerGoldCost;
            CumulativeWorkerGoldCost = cumulativeWorkerGoldCost;
        }
    }
}

