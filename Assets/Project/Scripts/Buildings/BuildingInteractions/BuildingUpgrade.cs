using Configs;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Building))]
[RequireComponent(typeof(BuildingExperience))]
[RequireComponent(typeof(BuildingConstruction))]
public class BuildingUpgrade : ConfigurableComponent
{
    private Building building;
    private BuildingExperience buildingExperience;
    private BuildingConstruction buildingConstruction;
    private ResourceManager resourceManager;
    private AltarFlameSystem altarFlameSystem;

    private BaseUpgradeConfig[] upgradeConfigs;
    private BaseUpgradeConfig currentUpgradeConfig;

    private const string CONFIG_TYPE_PREFIX = "Configs.";
    private const string CONFIG_TYPE_POSTFIX = "UpgradeConfig";

    public int CurrentUpgradeLevel { get; private set; } = 1;
    public bool IsMaxUpgradeLevel => upgradeConfigs == null || CurrentUpgradeLevel >= upgradeConfigs.Length;

    private void Awake()
    {
        building = GetComponent<Building>();
        buildingExperience = GetComponent<BuildingExperience>();
        buildingConstruction = GetComponent<BuildingConstruction>();
    }

    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
        altarFlameSystem = ServiceLocator.Get<AltarFlameSystem>();

        LoadConfigs();
    }

    protected override void LoadConfigs()
    {
        string typeName = $"{CONFIG_TYPE_PREFIX}{building.BuildingID}{CONFIG_TYPE_POSTFIX}";
        Type type = Type.GetType(typeName);

        if (type != null)
        {
            var configsArray = Configs.GetConfigs(type);
            if (configsArray != null)
            {
                upgradeConfigs = configsArray.OfType<BaseUpgradeConfig>().ToArray();
            }
        }

        UpdateCurrentConfig();
    }

    private void UpdateCurrentConfig()
    {
        currentUpgradeConfig = upgradeConfigs?.FirstOrDefault(c => c.CurrentUpgradeLevel == CurrentUpgradeLevel);
    }

    public bool CanUpgrade()
    {
        if (currentUpgradeConfig == null || IsMaxUpgradeLevel) return false;

        int altarLevel = altarFlameSystem?.CurrentLevel ?? 0;
        if (altarLevel < currentUpgradeConfig.AltarLevelRequired) return false;

        if (buildingExperience.CurrentLevel < currentUpgradeConfig.ExperienceLevelRequired) return false;

        if (!resourceManager.HasEnough(ResourceType.Gold, currentUpgradeConfig.UpgradeGoldCost)) return false;
        if (!resourceManager.HasEnough(ResourceType.Wood, currentUpgradeConfig.UpgradeWoodCost)) return false;
        if (!resourceManager.HasEnough(ResourceType.Stone, currentUpgradeConfig.UpgradeStoneCost)) return false;
        if (!resourceManager.HasEnough(ResourceType.Shards, currentUpgradeConfig.UpgradeShardsCost)) return false;

        return true;
    }

    public void StartUpgrade()
    {
        if (!CanUpgrade() || buildingConstruction == null) return;

        resourceManager.TrySpend(ResourceType.Gold, currentUpgradeConfig.UpgradeGoldCost);
        resourceManager.TrySpend(ResourceType.Wood, currentUpgradeConfig.UpgradeWoodCost);
        resourceManager.TrySpend(ResourceType.Stone, currentUpgradeConfig.UpgradeStoneCost);
        resourceManager.TrySpend(ResourceType.Shards, currentUpgradeConfig.UpgradeShardsCost);

        buildingConstruction.StartConstruction(currentUpgradeConfig.UpgradeTime, OnUpgradeComplete);
    }

    private void OnUpgradeComplete()
    {
        CurrentUpgradeLevel++;
        UpdateCurrentConfig();
    }

    public int GetRequiredAltarLevel()
    {
        return currentUpgradeConfig?.AltarLevelRequired ?? 0;
    }

    public int GetRequiredExperienceLevel()
    {
        return currentUpgradeConfig?.ExperienceLevelRequired ?? 0;
    }

    public int GetResourceCost(ResourceType resource)
    {
        if (currentUpgradeConfig == null) return 0;
        switch (resource)
        {
            case ResourceType.Gold: return currentUpgradeConfig.UpgradeGoldCost;
            case ResourceType.Wood: return currentUpgradeConfig.UpgradeWoodCost;
            case ResourceType.Stone: return currentUpgradeConfig.UpgradeStoneCost;
            case ResourceType.Shards: return currentUpgradeConfig.UpgradeShardsCost;
            default: return 0;
        }
    }
}