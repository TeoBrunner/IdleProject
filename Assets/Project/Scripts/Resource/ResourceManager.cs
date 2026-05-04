using Events;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private readonly Dictionary<ResourceType, float> balances = new();

    private void Awake()
    {
        ServiceLocator.Register<ResourceManager>(this);
    }

    public float GetBalance(ResourceType resource)
    {
        return balances.TryGetValue(resource, out float balance) ? balance : 0;
    }
    public void Add(ResourceType resource, float amount = 0, object source = null)
    {

        if (amount <= 0)
        {
            Debug.LogWarning($"ResourceManager.Add: amount must be non-negative. Trying to add: {amount} of {resource}");
            return;
        }

        if (!balances.ContainsKey(resource))
        {
            balances[resource] = 0;
        }

        balances[resource] += amount;
        EventBus.Publish(new ResourceAddedEvent(resource, amount, source));
        EventBus.Publish(new ResourceBalanceChangedEvent(resource, balances[resource]));
    }
    public bool TrySpend(ResourceType resource, float amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"ResourceManager.TrySpend: amount must be non-negative. Trying to spend: {amount} of {resource}");
            return false;
        }

        if (GetBalance(resource) < amount)
            return false;

        balances[resource] -= amount;
        EventBus.Publish(new ResourceSpendEvent(resource, amount));
        EventBus.Publish(new ResourceBalanceChangedEvent(resource, balances[resource]));
        return true;
    }
    public bool HasEnough(ResourceType resource, float amount)
    {
        return GetBalance(resource) >= amount;
    }
}
