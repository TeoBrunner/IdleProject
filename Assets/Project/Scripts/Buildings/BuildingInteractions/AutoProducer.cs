using Configs;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class AutoProducer : MonoBehaviour, IBuildingInteractionHandler
{
    [SerializeField] private ResourceType producedResource = ResourceType.Gold;

    private Building building;
    private ResourceManager resourceManager;
    private TownHallConstantsConfig[] constantsConfigs;

    private bool isPlayerNearby;
    private float timer;

    private float gatherPerAutoClick;
    private float autoClickInterval;

    private const string GATHER_PER_AUTO_CLICK_KEY = "gather_per_auto_click";
    private const string AUTO_CLICK_INTERVAL_KEY = "auto_click_interval";

    public ResourceType ProducedResource => producedResource;
    public float GatherPerAutoClick => gatherPerAutoClick;
    public float AutoClickInterval => autoClickInterval;

    private void Awake()
    {
        building = GetComponent<Building>();
    }

    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();

        var configProvider = ServiceLocator.Get<ConfigProvider>();
        constantsConfigs = configProvider.GetConfigs<TownHallConstantsConfig>();

        gatherPerAutoClick = constantsConfigs.GetConstant(GATHER_PER_AUTO_CLICK_KEY);
        autoClickInterval = constantsConfigs.GetConstant(AUTO_CLICK_INTERVAL_KEY);
    }

    private void Update()
    {
        if (!isPlayerNearby) return;
        if (gatherPerAutoClick <= 0 || autoClickInterval <= 0) return;

        timer += Time.deltaTime;

        if (timer >= gatherPerAutoClick)
        {
            timer = 0f;
            Produce();
        }
    }

    private void Produce()
    {
        resourceManager.Add(producedResource, gatherPerAutoClick, source: building);
    }

    public void OnPlayerEnter()
    {
        isPlayerNearby = true;
        timer = 0f; 
    }

    public void OnPlayerExit()
    {
        isPlayerNearby = false;
        timer = 0f;
    }

    public void OnInteract() { }

    public void OnExamine() { }
}