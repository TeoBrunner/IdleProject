using Configs;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class ManualProducer : MonoBehaviour, IBuildingInteractionHandler
{
    [SerializeField] private ResourceType producedResource = ResourceType.Gold;

    private const string GATHER_PER_CLICK_KEY = "gather_per_click";

    private Building building;
    private ResourceManager resourceManager;
    private TownHallConstantsConfig[] constantsConfigs;

    private float gatherPerClick;
    public ResourceType ProducedResource => producedResource;
    public float GatherPerClick => gatherPerClick;

    private void Awake()
    {
        building = GetComponent<Building>();
    }

    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();

        var configProvider = ServiceLocator.Get<ConfigProvider>();
        constantsConfigs = configProvider.GetConfigs<TownHallConstantsConfig>();

        gatherPerClick = constantsConfigs.GetConstant(GATHER_PER_CLICK_KEY);
    }
    public void OnPlayerEnter() { }
    public void OnPlayerExit() { }
    public void OnInteract()
    {
        if (resourceManager == null || constantsConfigs == null) return;

        if (gatherPerClick <= 0) return;

        resourceManager.Add(producedResource, gatherPerClick, source: building);
    }
    public void OnExamine() { }
}