using Configs;
using Events;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class BuildingWorkers : ConfigurableComponent
{
    [SerializeField] private ResourceType producedResource = ResourceType.Gold;

    private Building building;
    private BuildingUpgrade buildingUpgrade;
    private ResourceManager resourceManager;

    private BaseWorkersConfig[] workersConfigs;
    private BaseWorkersConfig currentConfig;

    private const string CONFIG_TYPE_PREFIX = "Configs.";
    private const string CONFIG_TYPE_POSTFIX = "WorkersConfig";

    private int currentWorkers = 0;
    private float productionTimer;

    public int CurrentWorkers => currentWorkers;
    public int MaxWorkers => currentConfig?.MaxWorkers ?? 0;
    public ResourceType ProducedResourceType => producedResource;
    public BaseWorkersConfig CurrentWorkerConfig => currentConfig;
    public bool CanAddWorker => currentWorkers < MaxWorkers && resourceManager.HasEnough(ResourceType.Workers, 1);
    public bool CanRemoveWorker => currentWorkers > 0;

    public event Action OnWorkersChanged;

    private void Awake()
    {
        building = GetComponent<Building>();
        buildingUpgrade = GetComponent<BuildingUpgrade>();
    }

    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
        LoadConfigs();

        if (buildingUpgrade != null)
            EventBus.Subscribe<BuildingUpgradedEvent>(OnBuildingUpgraded);
    }

    private void OnDestroy()
    {
        if (buildingUpgrade != null)
            EventBus.Unsubscribe<BuildingUpgradedEvent>(OnBuildingUpgraded);
    }

    protected override void LoadConfigs()
    {
        string typeName = $"{CONFIG_TYPE_PREFIX}{building.BuildingID}{CONFIG_TYPE_POSTFIX}";
        Type type = Type.GetType(typeName);
        if (type != null)
        {
            var configsArray = Configs.GetConfigs(type);
            if (configsArray != null)
                workersConfigs = configsArray.OfType<BaseWorkersConfig>().ToArray();
        }
        UpdateCurrentConfig();
    }

    private void UpdateCurrentConfig()
    {
        int upgradeLevel = buildingUpgrade != null ? buildingUpgrade.CurrentUpgradeLevel : 1;
        currentConfig = workersConfigs?.FirstOrDefault(c => c.CurrentUpgradeLevel == upgradeLevel);

        OnWorkersChanged?.Invoke();
    }

    private void OnBuildingUpgraded(BuildingUpgradedEvent e)
    {
        if (e.Building == building)
            UpdateCurrentConfig();
    }

    public void AddWorker()
    {
        if (!CanAddWorker) return;

        if (resourceManager.TrySpend(ResourceType.Workers, 1))
        {
            currentWorkers++;
            OnWorkersChanged?.Invoke();
        }
    }

    public void RemoveWorker()
    {
        if (!CanRemoveWorker) return;

        currentWorkers--;
        resourceManager.Add(ResourceType.Workers, 1, source: building);
        OnWorkersChanged?.Invoke();
    }

    private void Update()
    {
        if (currentWorkers <= 0 || currentConfig == null || !building.IsEnabled)
            return;

        if (currentConfig.WorkerProductionInterval <= 0f)
            return;

        productionTimer += Time.deltaTime;
        if (productionTimer >= currentConfig.WorkerProductionInterval)
        {
            productionTimer = 0f;
            Produce();
        }
    }

    private void Produce()
    {
        float totalProduction = currentWorkers * currentConfig.WorkerProduction;
        resourceManager.Add(producedResource, totalProduction, source: building);
    }
}