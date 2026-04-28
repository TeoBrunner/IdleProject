using Events;
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

        EventBus.Subscribe<ResourceBalanceChangedEvent>(HandleBalanceChanged);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ResourceBalanceChangedEvent>(HandleBalanceChanged);
    }

    private void HandleBalanceChanged(ResourceBalanceChangedEvent evt)
    {
        if (entries.TryGetValue(evt.ResourceType, out var existingEntry))
        {
            existingEntry.UpdateAmount(evt.Amount);
        }
        else
        {
            var entry = Instantiate(entryPrefab, transform);
            entry.Initialize(evt.ResourceType, evt.Amount);
            entries[evt.ResourceType] = entry;

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}
