using Events;
using TMPro;
using UnityEngine;

public class FoundationInfoPopup : LocalizedComponent
{
    [Header("Texts")]
    [SerializeField] private TMP_Text buildingNameText;

    [Header("Requirements")]
    [SerializeField] private BuildingUpgradeRequirementView altarRequirement;
    [SerializeField] private BuildingUpgradeRequirementView goldRequirement;
    [SerializeField] private BuildingUpgradeRequirementView woodRequirement;
    [SerializeField] private BuildingUpgradeRequirementView stoneRequirement;
    [SerializeField] private BuildingUpgradeRequirementView shardsRequirement;

    private ResourceManager resourceManager;
    private AltarFlameSystem altarFlameSystem;

    private const string ALTAR_LEVEL_KEY = "requirement_altar_level";

    // Кэшируем не просто строку, а ключ локализации
    private string cachedBuildingNameKey;

    private float cachedAltarRequired;
    private float cachedGoldRequired;
    private float cachedWoodRequired;
    private float cachedStoneRequired;
    private float cachedShardsRequired;

    protected override void RefreshLocalization()
    {
        Redraw();
    }

    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
        altarFlameSystem = ServiceLocator.Get<AltarFlameSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe<ResourceBalanceChangedEvent>(OnResourceBalanceChanged);
        EventBus.Subscribe<AltarFlameChangedEvent>(OnAltarFlameChanged);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.Unsubscribe<ResourceBalanceChangedEvent>(OnResourceBalanceChanged);
        EventBus.Unsubscribe<AltarFlameChangedEvent>(OnAltarFlameChanged);
    }

    public void Show(BuildingFoundation foundation)
    {
        if (foundation == null) return;

        cachedBuildingNameKey = foundation.BuildingPrefab.GetComponent<BuildingIdentity>().NameKey;
        cachedAltarRequired = foundation.AltarLevelRequired;
        cachedGoldRequired = foundation.GoldCost;
        cachedWoodRequired = foundation.WoodCost;
        cachedStoneRequired = foundation.StoneCost;
        cachedShardsRequired = foundation.ShardsCost;

        gameObject.SetActive(true);
        Redraw();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnResourceBalanceChanged(ResourceBalanceChangedEvent e) => Redraw();
    private void OnAltarFlameChanged(AltarFlameChangedEvent e) => Redraw();

    private void Redraw()
    {
        if (!gameObject.activeSelf) return;

        if (buildingNameText != null)
            buildingNameText.text = Localization.GetString(cachedBuildingNameKey);

        int altarCurrent = altarFlameSystem?.CurrentLevel ?? 0;
        string altarLabel = Localization.GetString(ALTAR_LEVEL_KEY);
        UpdateRequirement(altarRequirement, altarLabel, altarCurrent, cachedAltarRequired);

        int goldCurrent = (int)(resourceManager?.GetBalance(ResourceType.Gold) ?? 0);
        string goldLabel = Localization.GetString(ResourceType.Gold.ToString().ToLower());
        UpdateRequirement(goldRequirement, goldLabel, goldCurrent, cachedGoldRequired);

        int woodCurrent = (int)(resourceManager?.GetBalance(ResourceType.Wood) ?? 0);
        string woodLabel = Localization.GetString(ResourceType.Wood.ToString().ToLower());
        UpdateRequirement(woodRequirement, woodLabel, woodCurrent, cachedWoodRequired);

        int stoneCurrent = (int)(resourceManager?.GetBalance(ResourceType.Stone) ?? 0);
        string stoneLabel = Localization.GetString(ResourceType.Stone.ToString().ToLower());
        UpdateRequirement(stoneRequirement, stoneLabel, stoneCurrent, cachedStoneRequired);

        int shardsCurrent = (int)(resourceManager?.GetBalance(ResourceType.Shards) ?? 0);
        string shardsLabel = Localization.GetString(ResourceType.Shards.ToString().ToLower());
        UpdateRequirement(shardsRequirement, shardsLabel, shardsCurrent, cachedShardsRequired);
    }

    private void UpdateRequirement(BuildingUpgradeRequirementView view,
        string label, float current, float required)
    {
        if (view == null) return;

        if (required <= 0)
        {
            view.gameObject.SetActive(false);
            return;
        }

        view.gameObject.SetActive(true);
        bool met = current >= required;
        view.UpdateDisplay($"{label}: {current}/{required}", met);
    }
}