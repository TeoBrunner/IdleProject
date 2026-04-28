using UnityEngine;

[RequireComponent(typeof(Building))]
public class BuildingIdentity : MonoBehaviour, IBuildingInteractionHandler
{
    [SerializeField] private string nameKey;
    [SerializeField] private string descriptionKey;
    public string NameKey => nameKey;
    public string DescriptionKey => descriptionKey;
    public void OnInteract()
    {
    }

    public void OnPlayerEnter()
    {
    }

    public void OnPlayerExit()
    {
    }

}
