using Events;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private readonly Dictionary<ResourceType, int> balances = new();

    private void Awake()
    {
        ServiceLocator.Register<ResourceManager>(this);
    }

    public int GetBalance(ResourceType resource)
    {
        return balances.TryGetValue(resource, out int balance) ? balance : 0;
    }
    public void Add(ResourceType resource, int amount = 0)
    {

        if (amount <= 0)
        {
            Debug.LogWarning($"ResourceManager.Add: amount must be non-negative. Trying to add: {amount}");
            return;
        }

        if (!balances.ContainsKey(resource))
        {
            balances[resource] = 0;
        }

        balances[resource] += amount;
        EventBus.Publish(new ResourceAddedEvent(resource, amount));
        EventBus.Publish(new ResourceBalanceChangedEvent(resource, balances[resource]));
    }
    public bool TrySpend(ResourceType resource, int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"ResourceManager.TrySpend: amount must be non-negative. Trying to spend: {amount}");
            return false;
        }

        if (GetBalance(resource) < amount)
            return false;

        balances[resource] -= amount;
        EventBus.Publish(new ResourceSpendEvent(resource, amount));
        EventBus.Publish(new ResourceBalanceChangedEvent(resource, balances[resource]));
        return true;
    }
    public bool HasEnough(ResourceType resource, int amount)
    {
        return GetBalance(resource) >= amount;
    }
}
