using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgradeInfoBlock : MonoBehaviour, IBuildingInfoBlock
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
    private LocalizationProvider localizationProvider;

    private const string ALTAR_LEVEL_KEY = "requirement_altar_level";
    private const string BUILDING_LEVEL_KEY = "requirement_building_level";
    private const string GOLD_KEY = "gold";
    private const string WOOD_KEY = "wood";
    private const string STONE_KEY = "stone";
    private const string SHARDS_KEY = "shards";

    private void Awake()
    {
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }

    private void OnDestroy()
    {
        upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
    }

    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
        altarFlameSystem = ServiceLocator.Get<AltarFlameSystem>();
        localizationProvider = ServiceLocator.Get<LocalizationProvider>();
    }

    void OnEnable()
    {
        if (!localizationProvider) return;
        localizationProvider.LocalizationUpdated += OnLocalizationUpdated;
    }

    void OnDisable()
    {
        if (!localizationProvider) return;
        localizationProvider.LocalizationUpdated -= OnLocalizationUpdated;
    }

    private void OnLocalizationUpdated()
    {
        UpdateAllRequirements();
        UpdateUpgradeButton();
    }

    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<BuildingUpgrade>(out var upgrade))
        {
            if (resourceManager == null) resourceManager = ServiceLocator.Get<ResourceManager>();
            if (altarFlameSystem == null) altarFlameSystem = ServiceLocator.Get<AltarFlameSystem>();
            if (localizationProvider == null) localizationProvider = ServiceLocator.Get<LocalizationProvider>();

            currentBuilding = building;
            buildingUpgrade = upgrade;
            buildingExperience = building.GetComponent<BuildingExperience>();
            gameObject.SetActive(true);

            EventBus.Subscribe<AltarFlameChangedEvent>(OnAltarFlameChanged);
            EventBus.Subscribe<BuildingExperienceChangedEvent>(OnBuildingExperienceChanged);
            EventBus.Subscribe<ResourceBalanceChangedEvent>(OnResourceBalanceChanged);

            UpdateAllRequirements();
            UpdateUpgradeButton();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPanelClose()
    {
        gameObject.SetActive(false);

        EventBus.Unsubscribe<AltarFlameChangedEvent>(OnAltarFlameChanged);
        EventBus.Unsubscribe<BuildingExperienceChangedEvent>(OnBuildingExperienceChanged);
        EventBus.Unsubscribe<ResourceBalanceChangedEvent>(OnResourceBalanceChanged);

        currentBuilding = null;
        buildingUpgrade = null;
        buildingExperience = null;
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
        string label = localizationProvider.GetString(ALTAR_LEVEL_KEY);
        altarLevelRequirement.UpdateDisplay($"{label}: {current} / {required}", met);
    }

    private void UpdateBuildingLevelRequirement()
    {
        if (buildingExperience == null || buildingUpgrade == null) return;
        if (!localizationProvider)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();
        int current = buildingExperience.CurrentLevel;
        int required = buildingUpgrade.GetRequiredExperienceLevel();
        bool met = current >= required;
        string label = localizationProvider.GetString(BUILDING_LEVEL_KEY);
        buildingLevelRequirement.UpdateDisplay($"{label}: {current} / {required}", met);
    }

    private void UpdateResourceRequirements()
    {
        if (buildingUpgrade == null || resourceManager == null) return;
        if (!localizationProvider)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();
        UpdateResourceRequirement(ResourceType.Gold, goldRequirement, GOLD_KEY);
        UpdateResourceRequirement(ResourceType.Wood, woodRequirement, WOOD_KEY);
        UpdateResourceRequirement(ResourceType.Stone, stoneRequirement, STONE_KEY);
        UpdateResourceRequirement(ResourceType.Shards, shardsRequirement, SHARDS_KEY);
    }

    private void UpdateResourceRequirement(ResourceType type, BuildingUpgradeRequirementView view, string resourceKey)
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
        string resourceName = localizationProvider.GetString(resourceKey);
        view.UpdateDisplay($"{resourceName}: {current} / {required}", met);
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