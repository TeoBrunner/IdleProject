using Configs;
using UnityEngine;
[RequireComponent(typeof(Building))]
public class AutoClickProducer : MonoBehaviour, IBuildingInteractionHandler
{
    private Building building;
    private ResourceManager resourceManager;

    private bool isPlayerNearby;
    private float tickTimer;

    private BuildingMainConfig Config => building.Configs[building.Level];
    private void Awake()
    {
        building = GetComponent<Building>();
    }
    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    private void Update()
    {
        if (!isPlayerNearby) return;
        if (!Config.HasAutoClick) return;

        tickTimer += Time.deltaTime;

        if (tickTimer >= Config.AutoClickInterval)
        {
            tickTimer -= Config.AutoClickInterval;
            Produce();
        }
    }

    private void Produce()
    {
        resourceManager.Add(Config.ProducedResource, Config.GatherPerAutoClick);
    }

    public void OnPlayerEnter()
    {
        isPlayerNearby = true;
        tickTimer = 0f;
    }

    public void OnPlayerExit()
    {
        isPlayerNearby = false;
        tickTimer = 0f;
    }

    public void OnInteract() { }
}
