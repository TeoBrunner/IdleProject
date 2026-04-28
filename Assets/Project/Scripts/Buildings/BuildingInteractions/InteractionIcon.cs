using UnityEngine;

[RequireComponent(typeof(Building))]
public class InteractionIcon : MonoBehaviour, IBuildingInteractionHandler
{
    [SerializeField] private GameObject icon;
    public void OnPlayerEnter()
    {
        icon.SetActive(true);
    }
    public void OnPlayerExit()
    {
        icon.SetActive(false);
    }
    public void OnInteract() { }
    public void OnExamine() { }
}