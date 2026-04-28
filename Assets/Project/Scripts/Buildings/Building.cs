using Configs;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IInteractable
{
    [SerializeField] string buildingID;
    private IBuildingInteractionHandler[] interactionHandlers;

    private void Awake()
    {
        interactionHandlers = GetComponents<IBuildingInteractionHandler>();
    }
    public void OnPlayerEnter()
    {
        foreach (var handler in interactionHandlers)
            handler.OnPlayerEnter();
    }

    public void OnPlayerExit()
    {
        foreach (var handler in interactionHandlers)
            handler.OnPlayerExit();
    }

    public void OnInteract()
    {
        foreach (var handler in interactionHandlers)
            handler.OnInteract();
    }
}
