using System.Collections.Generic;
using UnityEngine;

public class ResourcePanel : MonoBehaviour
{
    [SerializeField] private ResourceEntryView entryPrefab;

    private ResourceManager resourceManager;

    private readonly Dictionary<ResourceDefinition, ResourceEntryView> entries = new();

    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();

        if (resourceManager == null) return;

        resourceManager.OnBalanceChanged += HandleBalanceChanged;
    }

    private void OnDestroy()
    {
        if (resourceManager != null)
            resourceManager.OnBalanceChanged -= HandleBalanceChanged;
    }

    private void HandleBalanceChanged(ResourceDefinition resource, int newBalance)
    {
        if (entries.TryGetValue(resource, out var existingEntry))
        {
            existingEntry.UpdateAmount(newBalance);
        }
        else
        {
            var entry = Instantiate(entryPrefab, transform);
            entry.Initialize(resource, newBalance);
            entries[resource] = entry;
        }
    }
}
