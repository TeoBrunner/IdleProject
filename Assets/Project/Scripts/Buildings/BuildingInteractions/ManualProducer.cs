using Configs;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class ManualProducer : MonoBehaviour, IBuildingInteractionHandler
{
    private Building building;
    private ResourceManager resourceManager;
    //private BuildingMainConfig Config => building.Configs[building.Level];

    private void Awake()
    {
        building = GetComponent<Building>();
    }
    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void OnInteract()
    {
        //if (!Config.HasManualClick) return;
        //resourceManager.Add(Config.ProducedResource, Config.GatherPerClick);
    }

    public void OnPlayerEnter() {}

    public void OnPlayerExit() {}
}
