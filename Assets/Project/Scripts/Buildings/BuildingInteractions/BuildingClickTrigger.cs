using Events;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class BuildingClickTrigger : MonoBehaviour, IBuildingInteractionHandler
{
    private Building building;

    private void Awake()
    {
        building = GetComponent<Building>();
    }
    public void OnPlayerEnter() { }
    public void OnPlayerExit() { }
    public void OnInteract()
    {
        EventBus.Publish(new BuildingClickedEvent(building));
    }
    public void OnExamine() { }

}