using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour
{
    [SerializeField] private ResourceEntryView entryPrefab;

    private ResourceManager resourceManager;

    private readonly Dictionary<ResourceType, ResourceEntryView> entries = new();

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

    private void HandleBalanceChanged(ResourceType resource, int newBalance)
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

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}
