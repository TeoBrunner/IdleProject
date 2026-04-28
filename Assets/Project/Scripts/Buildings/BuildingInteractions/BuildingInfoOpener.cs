using UnityEngine;

[RequireComponent(typeof(Building))]
public class BuildingInfoOpener : MonoBehaviour, IBuildingInteractionHandler
{
    private Building building;
    private BuildingInfoPanel infoPanel;
    private void Start()
    {
        building = GetComponent<Building>();
        infoPanel = ServiceLocator.Get<BuildingInfoPanel>();
    }
    public void OnInteract()
    {
        infoPanel.Show(building);
    }

    public void OnPlayerEnter()
    {

    }

    public void OnPlayerExit()
    {
        infoPanel.Hide();
    }
}
