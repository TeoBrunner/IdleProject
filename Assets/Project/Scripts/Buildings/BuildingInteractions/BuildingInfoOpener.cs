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
    public void OnPlayerEnter() { }
    public void OnPlayerExit()
    {
        infoPanel.Hide();
    }
    public void OnInteract() { }
    public void OnExamine() 
    {
        infoPanel.Show(building);
    }
}
