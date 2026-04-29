using Configs;
using Events;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Building))]
[RequireComponent(typeof(BuildingConstruction))]
public class AltarFlameSystem : MonoBehaviour, IBuildingInteractionHandler
{
    private AltarConstantsConfig[] constantsConfigs;
    private AltarFlameConfig[] flameConfigs;
    private AltarUpgradeConfig[] upgradeConfigs;

    private BuildingConstruction buildingConstruction;

    private int currentLevel = 1;
    private float currentFlame = 0f;

    private const string FLAME_PER_CLICK_KEY = "flame_per_click";
    private const string FLAME_PER_GOLD_KEY = "flame_per_gold";

    public int CurrentLevel => currentLevel;
    public float CurrentFlame => currentFlame;
    public int RequiredFlame => flameConfigs?.FirstOrDefault(c => c.CurrentUpgradeLevel == currentLevel)?
                                      .FlamePointsRequired ?? 0;
    public bool CanUpgrade => currentFlame >= RequiredFlame;

    private void Awake()
    {
        buildingConstruction = GetComponent<BuildingConstruction>();

        ServiceLocator.Register<AltarFlameSystem>(this);
    }
    private void Start()
    {
        var configProvider = ServiceLocator.Get<ConfigProvider>();
        constantsConfigs = configProvider.GetConfigs<AltarConstantsConfig>();
        flameConfigs = configProvider.GetConfigs<AltarFlameConfig>();
        upgradeConfigs = configProvider.GetConfigs<AltarUpgradeConfig>();

        EventBus.Subscribe<BuildingClickedEvent>(OnBuildingClicked);
        EventBus.Subscribe<ResourceAddedEvent>(OnResourceAdded);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<BuildingClickedEvent>(OnBuildingClicked);
        EventBus.Unsubscribe<ResourceAddedEvent>(OnResourceAdded);
    }

    private void OnBuildingClicked(BuildingClickedEvent e)
    {
        float perClick = constantsConfigs.GetConstant(FLAME_PER_CLICK_KEY);
        AddFlame(perClick);
    }

    private void OnResourceAdded(ResourceAddedEvent e)
    {
        if (e.ResourceType == ResourceType.Gold)
        {
            float perGold = constantsConfigs.GetConstant(FLAME_PER_GOLD_KEY);
            AddFlame(e.Amount * perGold);
        }
    }

    private void AddFlame(float amount)
    {
        if (CanUpgrade) return;
        currentFlame = Mathf.Min(currentFlame + amount, RequiredFlame);

        EventBus.Publish(new AltarFlameChangedEvent(currentFlame, RequiredFlame, currentLevel));
    }
    public void StartUpgrade()
    {
        if (!CanUpgrade || buildingConstruction == null) return;

        var upgradeConfig = upgradeConfigs?.FirstOrDefault(c => c.CurrentUpgradeLevel == currentLevel);
        int buildTime = upgradeConfig.UpgradeTime;

        buildingConstruction.StartConstruction(buildTime, OnUpgradeComplete);
    }

    private void OnUpgradeComplete()
    {
        currentLevel++;
        currentFlame = 0f;

        EventBus.Publish(new AltarFlameChangedEvent(currentFlame, RequiredFlame, currentLevel));
    }
    public void OnPlayerEnter() { }
    public void OnPlayerExit() { }
    public void OnInteract() { }
    public void OnExamine() { }
}
