using UnityEngine;
[RequireComponent(typeof(Building))]
public class AutoClickProducer : MonoBehaviour, IBuildingInteractionHandler
{
    private BuildingData data;
    private ResourceManager resourceManager;

    private bool isPlayerNearby;
    private float tickTimer;

    private void Awake()
    {
        data = GetComponent<Building>().Data;
    }
    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    private void Update()
    {
        if (!isPlayerNearby) return;
        if (data.ProducedResource == null) return;

        tickTimer += Time.deltaTime;

        if (tickTimer >= data.TickInterval)
        {
            tickTimer -= data.TickInterval;
            Produce();
        }
    }

    private void Produce()
    {
        resourceManager.Add(data.ProducedResource, data.ResourcePerTick);
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
