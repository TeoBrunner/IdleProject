using Configs;
using Events;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class AltarFlameSystem : MonoBehaviour, IBuildingInteractionHandler
{
    private AltarConstantsConfig[] constantsConfigs;
    private AltarFlameConfig[] flameConfigs;

    private int currentLevel = 1;
    private float currentFlame = 0f;

    private const string FLAME_PER_CLICK_KEY = "flame_per_click";
    private const string FLAME_PER_GOLD_KEY = "flame_per_gold";

    public int CurrentLevel => currentLevel;
    public float CurrentFlame => currentFlame;
    public int RequiredFlame => flameConfigs?.FirstOrDefault(c => c.CurrentUpgradeLevel == currentLevel)?
                                      .FlamePointsRequired ?? 0;
    public bool CanUpgrade => currentFlame >= RequiredFlame;

    private void Start()
    {
        var configProvider = ServiceLocator.Get<ConfigProvider>();
        constantsConfigs = configProvider.GetConfigs<AltarConstantsConfig>();
        flameConfigs = configProvider.GetConfigs<AltarFlameConfig>();

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

    public void OnPlayerEnter() { }
    public void OnPlayerExit() { }
    public void OnInteract() { }
    public void OnExamine() { }
}
