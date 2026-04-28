using Configs;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IInteractable
{
    private IBuildingInteractionHandler[] interactionHandlers;

    private ConfigProvider configProvider;
    private BuildingMainConfig[] configs;
    private int level;

    public BuildingMainConfig[] Configs => configs;
    public int Level => level;
    private void Awake()
    {
        interactionHandlers = GetComponents<IBuildingInteractionHandler>();
    }
    private void Start()
    {
        configProvider = ServiceLocator.Get<ConfigProvider>();
        if (configProvider)
        {
            UpdateConfig();
            configProvider.ConfigUpdated += UpdateConfig;
        }
    }
    private void UpdateConfig()
    {
        if (configProvider)
        {
            configs = configProvider.GetConfigs<BuildingMainConfig>();
        }
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
