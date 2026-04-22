using UnityEngine;

[RequireComponent(typeof(Building))]
public class ManualProducer : MonoBehaviour, IBuildingInteractionHandler
{
    private BuildingData data;
    private ResourceManager resourceManager;

    private void Awake()
    {
        data = GetComponent<Building>().Data;
    }
    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void OnInteract()
    {
        resourceManager.Add(data.ProducedResource, data.ResourcePerClick);
    }

    public void OnPlayerEnter() {}

    public void OnPlayerExit() {}
}
