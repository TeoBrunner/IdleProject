using Configs;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class ManualProducer : ConfigurableComponent, IBuildingInteractionHandler
{
    [SerializeField] private ResourceType producedResource = ResourceType.Gold;

    private const string GATHER_PER_CLICK_KEY = "gather_per_click";

    private Building building;
    private ResourceManager resourceManager;
    private TownHallConstantsConfig[] constantsConfigs;

    public ResourceType ProducedResource => producedResource;
    public float GatherPerClick => constantsConfigs.GetConstant(GATHER_PER_CLICK_KEY);

    private void Awake()
    {
        building = GetComponent<Building>();
    }

    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
        LoadConfigs();
    }

    protected override void LoadConfigs()
    {
        constantsConfigs = Configs.GetConfigs<TownHallConstantsConfig>();
    }

    public void OnPlayerEnter() { }
    public void OnPlayerExit() { }

    public void OnInteract()
    {
        if (resourceManager == null || constantsConfigs == null) return;

        if (GatherPerClick <= 0) return;

        resourceManager.Add(producedResource, GatherPerClick, source: building);
    }

    public void OnExamine() { }
}