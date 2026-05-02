using Configs;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class AutoProducer : ConfigurableComponent, IBuildingInteractionHandler
{
    [SerializeField] private ResourceType producedResource = ResourceType.Gold;

    private Building building;
    private ResourceManager resourceManager;
    private TownHallConstantsConfig[] constantsConfigs;

    private bool isPlayerNearby;
    private float timer;

    private const string GATHER_PER_AUTO_CLICK_KEY = "gather_per_auto_click";
    private const string AUTO_CLICK_INTERVAL_KEY = "auto_click_interval";

    public ResourceType ProducedResource => producedResource;
    public float GatherPerAutoClick => constantsConfigs.GetConstant(GATHER_PER_AUTO_CLICK_KEY);
    public float AutoClickInterval => constantsConfigs.GetConstant(AUTO_CLICK_INTERVAL_KEY);

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

    private void Update()
    {
        if (!isPlayerNearby) return;

        if (GatherPerAutoClick <= 0 || AutoClickInterval <= 0) return;

        timer += Time.deltaTime;

        if (timer >= AutoClickInterval)
        {
            timer = 0f;
            Produce();
        }
    }

    private void Produce()
    {
        resourceManager.Add(producedResource, GatherPerAutoClick, source: building);
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