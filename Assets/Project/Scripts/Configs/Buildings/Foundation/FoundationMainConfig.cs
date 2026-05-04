namespace Configs
{
    public class FoundationMainConfig
    {
        public readonly string BuildingID;
        public readonly float AltarLevelRequired;
        public readonly float ConstructionGoldCost;
        public readonly float ConstructionWoodCost;
        public readonly float ConstructionStoneCost;
        public readonly float ConstructionShardsCost;
        public readonly float ConstructionTime;

        public FoundationMainConfig(
            string buildingID,
            float altarLevelRequired,
            float constructionGoldCost,
            float constructionWoodCost,
            float constructionStoneCost,
            float constructionShardsCost,
            float constructionTime)
        {
            BuildingID = buildingID;
            AltarLevelRequired = altarLevelRequired;
            ConstructionGoldCost = constructionGoldCost;
            ConstructionWoodCost = constructionWoodCost;
            ConstructionStoneCost = constructionStoneCost;
            ConstructionShardsCost = constructionShardsCost;
            ConstructionTime = constructionTime;

        }
    }
}

