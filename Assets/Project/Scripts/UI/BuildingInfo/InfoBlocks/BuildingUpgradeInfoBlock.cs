using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgradeInfoBlock : LocalizedComponent, IBuildingInfoBlock
{
    [Header("Requirements")]
    [SerializeField] private BuildingUpgradeRequirementView altarLevelRequirement;
    [SerializeField] private BuildingUpgradeRequirementView buildingLevelRequirement;
    [SerializeField] private BuildingUpgradeRequirementView goldRequirement;
    [SerializeField] private BuildingUpgradeRequirementView woodRequirement;
    [SerializeField] private BuildingUpgradeRequirementView stoneRequirement;
    [SerializeField] private BuildingUpgradeRequirementView shardsRequirement;

    [Header("Button")]
    [SerializeField] private Button upgradeButton;

    private Building currentBuilding;
    private BuildingUpgrade buildingUpgrade;
    private BuildingExperience buildingExperience;
    private AltarFlameSystem altarFlameSystem;
    private ResourceManager resourceManager;

    private const string ALTAR_LEVEL_KEY = "requirement_altar_level";
    private const string BUILDING_LEVEL_KEY = "requirement_building_level";
    private const string GOLD_KEY = "gold";
    private const string WOOD_KEY = "wood";
    private const string STONE_KEY = "stone";
    private const string SHARDS_KEY = "shards";

    private string altarLevelLabel;
    private string buildingLevelLabel;
    private string goldLabel;
    private string woodLabel;
    private string stoneLabel;
    private string shardsLabel;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }

    private void OnDestroy()
    {
        upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
    }

    protected override void RefreshLocalization()
    {
        altarLevelLabel = Localization.GetString(ALTAR_LEVEL_KEY);
        buildingLevelLabel = Localization.GetString(BUILDING_LEVEL_KEY);
        goldLabel = Localization.GetString(GOLD_KEY);
        woodLabel = Localization.GetString(WOOD_KEY);
        stoneLabel = Localization.GetString(STONE_KEY);
        shardsLabel = Localization.GetString(SHARDS_KEY);

        if (buildingUpgrade != null)
        {
            UpdateAllRequirements();
            UpdateUpgradeButton();
        }
    }

    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<BuildingUpgrade>(out var upgrade))
        {
            currentBuilding = building;
            buildingUpgrade = upgrade;
            buildingExperience = building.GetComponent<BuildingExperience>();

            resourceManager = ServiceLocator.Get<ResourceManager>();
            altarFlameSystem = ServiceLocator.Get<AltarFlameSystem>();

            gameObject.SetActive(true);

            RefreshLocalization();

            EventBus.Subscribe<AltarFlameChangedEvent>(OnAltarFlameChanged);
            EventBus.Subscribe<BuildingExperienceChangedEvent>(OnBuildingExperienceChanged);
            EventBus.Subscribe<ResourceBalanceChangedEvent>(OnResourceBalanceChanged);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPanelClose()
    {
        EventBus.Unsubscribe<AltarFlameChangedEvent>(OnAltarFlameChanged);
        EventBus.Unsubscribe<BuildingExperienceChangedEvent>(OnBuildingExperienceChanged);
        EventBus.Unsubscribe<ResourceBalanceChangedEvent>(OnResourceBalanceChanged);

        currentBuilding = null;
        buildingUpgrade = null;
        buildingExperience = null;
        gameObject.SetActive(false);
    }

    private void OnAltarFlameChanged(AltarFlameChangedEvent e) => UpdateAltarRequirement();
    private void OnBuildingExperienceChanged(BuildingExperienceChangedEvent e)
    {
        if (e.Building == currentBuilding)
            UpdateBuildingLevelRequirement();
    }
    private void OnResourceBalanceChanged(ResourceBalanceChangedEvent e)
    {
        ResourceType type = e.ResourceType;
        if (type == ResourceType.Gold || type == ResourceType.Wood || type == ResourceType.Stone || type == ResourceType.Shards)
        {
            UpdateResourceRequirements();
            UpdateUpgradeButton();
        }
    }

    private void UpdateAllRequirements()
    {
        UpdateAltarRequirement();
        UpdateBuildingLevelRequirement();
        UpdateResourceRequirements();
    }

    private void UpdateAltarRequirement()
    {
        if (altarFlameSystem == null || buildingUpgrade == null) return;
        int current = altarFlameSystem.CurrentLevel;
        int required = buildingUpgrade.GetRequiredAltarLevel();
        bool met = current >= required;
        altarLevelRequirement.UpdateDisplay($"{altarLevelLabel}: {current} / {required}", met);
    }

    private void UpdateBuildingLevelRequirement()
    {
        if (buildingExperience == null || buildingUpgrade == null) return;
        int current = buildingExperience.CurrentLevel;
        int required = buildingUpgrade.GetRequiredExperienceLevel();
        bool met = current >= required;
        buildingLevelRequirement.UpdateDisplay($"{buildingLevelLabel}: {current} / {required}", met);
    }

    private void UpdateResourceRequirements()
    {
        if (buildingUpgrade == null || resourceManager == null) return;
        UpdateResourceRequirement(ResourceType.Gold, goldRequirement, goldLabel);
        UpdateResourceRequirement(ResourceType.Wood, woodRequirement, woodLabel);
        UpdateResourceRequirement(ResourceType.Stone, stoneRequirement, stoneLabel);
        UpdateResourceRequirement(ResourceType.Shards, shardsRequirement, shardsLabel);
    }

    private void UpdateResourceRequirement(ResourceType type, BuildingUpgradeRequirementView view, string resourceLabel)
    {
        if (view == null) return;

        int required = buildingUpgrade.GetResourceCost(type);

        if (required <= 0)
        {
            view.gameObject.SetActive(false);
            return;
        }

        view.gameObject.SetActive(true);
        int current = (int)resourceManager.GetBalance(type);
        bool met = current >= required;
        view.UpdateDisplay($"{resourceLabel}: {current} / {required}", met);
    }

    private void UpdateUpgradeButton()
    {
        if (upgradeButton == null || buildingUpgrade == null) return;
        upgradeButton.interactable = buildingUpgrade.CanUpgrade() && !buildingUpgrade.IsMaxUpgradeLevel;
    }

    public void OnUpgradeButtonClicked()
    {
        ServiceLocator.Get<BuildingInfoPanel>()?.Hide();
        buildingUpgrade?.StartUpgrade();
    }

}