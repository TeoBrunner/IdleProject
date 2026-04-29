using UnityEngine;
using System.Collections.Generic;

public class Building : MonoBehaviour, IInteractable
{
    [SerializeField] string buildingID;
    private IBuildingInteractionHandler[] interactionHandlers;

    public bool IsEnabled { get; private set; } = true;

    private void Awake()
    {
        interactionHandlers = GetComponents<IBuildingInteractionHandler>();
    }

    public void SetEnabled(bool enabled)
    {
        IsEnabled = enabled;
    }

    public void OnPlayerEnter()
    {
        if (!IsEnabled) return;
        foreach (var handler in interactionHandlers)
            handler.OnPlayerEnter();
    }

    public void OnPlayerExit()
    {
        if (!IsEnabled) return;
        foreach (var handler in interactionHandlers)
            handler.OnPlayerExit();
    }

    public void OnInteract()
    {
        if (!IsEnabled) return;
        foreach (var handler in interactionHandlers)
            handler.OnInteract();
    }

    public void OnExamine()
    {
        if (!IsEnabled) return;
        foreach (var handler in interactionHandlers)
            handler.OnExamine();
    }
}