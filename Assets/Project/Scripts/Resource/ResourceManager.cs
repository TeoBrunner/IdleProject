using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public event Action<ResourceDefinition, int> OnBalanceChanged;

    private readonly Dictionary<ResourceDefinition, int> _balances = new();

    private void Awake()
    {
        ServiceLocator.Register<ResourceManager>(this);
    }

    public int GetBalance(ResourceDefinition resource)
    {
        return _balances.TryGetValue(resource, out int balance) ? balance : 0;
    }
    public void Add(ResourceDefinition resource, int amount = 0)
    {
        if (resource == null)
        {
            Debug.LogWarning("ResourceManager.Add: resource is null");
            return;
        }

        if (amount <= 0)
        {
            Debug.LogWarning($"ResourceManager.Add: amount must be non-negative. Trying to add: {amount}");
            return;
        }

        if (!_balances.ContainsKey(resource))
        {
            _balances[resource] = 0;
        }

        _balances[resource] += amount;
        OnBalanceChanged?.Invoke(resource, _balances[resource]);
    }
    public bool TrySpend(ResourceDefinition resource, int amount)
    {
        if (resource == null)
        {
            Debug.LogWarning("ResourceManager.TrySpend: resource is null");
            return false;
        }

        if (amount <= 0)
        {
            Debug.LogWarning($"ResourceManager.TrySpend: amount must be non-negative. Trying to spend: {amount}");
            return false;
        }

        if (GetBalance(resource) < amount)
            return false;

        _balances[resource] -= amount;
        OnBalanceChanged?.Invoke(resource, _balances[resource]);
        return true;
    }
    public bool HasEnough(ResourceDefinition resource, int amount)
    {
        return GetBalance(resource) >= amount;
    }
}
