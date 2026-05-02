using Configs;
using Events;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class BuildingExperience : ConfigurableComponent, IBuildingInteractionHandler
{
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private float currentExp;

    private Building building;
    private TownHallConstantsConfig[] constantsConfigs;
    private TownHallExperienceConfig[] experienceConfigs;

    private const string EXP_PER_CLICK_KEY = "exp_per_click";
    private const string EXP_PER_CLICK_GOLD_KEY = "exp_per_click_gold";

    public int CurrentLevel => currentLevel;
    public int CurrentExpInt => Mathf.FloorToInt(currentExp);
    public int RequiredExp => experienceConfigs?.FirstOrDefault(c => c.ExpLevel == currentLevel)?.ExpPointsRequired ?? 0;
    public bool IsMaxLevel => currentLevel >= (experienceConfigs?.Length ?? 1);

    private void Awake()
    {
        building = GetComponent<Building>();
    }

    private void Start()
    {
        LoadConfigs();

        EventBus.Subscribe<BuildingClickedEvent>(OnBuildingClicked);
        EventBus.Subscribe<ResourceAddedEvent>(OnResourceAdded);

        PublishChangeEvent();
    }

    protected override void LoadConfigs()
    {
        constantsConfigs = Configs.GetConfigs<TownHallConstantsConfig>();
        experienceConfigs = Configs.GetConfigs<TownHallExperienceConfig>();
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<BuildingClickedEvent>(OnBuildingClicked);
        EventBus.Unsubscribe<ResourceAddedEvent>(OnResourceAdded);
    }

    private void OnBuildingClicked(BuildingClickedEvent e)
    {
        if (e.Building != building) return;

        float expPerClick = constantsConfigs.GetConstant(EXP_PER_CLICK_KEY);
        AddExperience(expPerClick);
    }

    private void OnResourceAdded(ResourceAddedEvent e)
    {
        if (e.Source is Building sourceBuilding && sourceBuilding != building) return;

        if (e.ResourceType == ResourceType.Gold)
        {
            float expPerGold = constantsConfigs.GetConstant(EXP_PER_CLICK_GOLD_KEY);
            AddExperience(e.Amount * expPerGold);
        }
    }

    private void AddExperience(float amount)
    {
        if (IsMaxLevel) return;
        if (amount <= 0) return;

        currentExp += amount;

        while (!IsMaxLevel && currentExp >= RequiredExp)
        {
            currentExp -= RequiredExp;
            currentLevel++;
        }

        if (!IsMaxLevel)
        {
            currentExp = Mathf.Min(currentExp, RequiredExp);
        }
        else
        {
            currentExp = 0;
        }

        PublishChangeEvent();
    }

    private void PublishChangeEvent()
    {
        EventBus.Publish(new BuildingExperienceChangedEvent(building, currentLevel, CurrentExpInt, RequiredExp));
    }

    public void OnPlayerEnter() { }
    public void OnPlayerExit() { }
    public void OnInteract() { }
    public void OnExamine() { }
}