using UnityEngine;
namespace Configs
{
    public class BuildingMainConfig
    {
        public readonly int Level;
        public readonly ResourceType ProducedResource;
        public readonly bool HasManualClick;
        public readonly int GatherPerClick;
        public readonly bool HasAutoClick;
        public readonly float AutoClickInterval;
        public readonly int GatherPerAutoClick;
        public BuildingMainConfig(
            int level, 
            ResourceType producedResource,
            bool hasManualClick,
            int gatherPerClick,
            bool hasAutoClick,
            float autoClickInterval,
            int gatherPerAutoClick)
        {
            Level = level;
            ProducedResource = producedResource;
            HasManualClick = hasManualClick;
            GatherPerClick = gatherPerClick;
            HasAutoClick = hasAutoClick;
            AutoClickInterval = autoClickInterval;
            GatherPerAutoClick = gatherPerAutoClick;
        }
    }
}

