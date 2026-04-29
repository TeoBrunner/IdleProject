using System;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class BuildingConstruction : MonoBehaviour
{
    [SerializeField] private GameObject defaultVisual;
    [SerializeField] private GameObject constructionVisual;

    private Building building;

    private bool isConstructing;
    private float timer;
    private float duration;
    private Action onComplete;

    private void Awake()
    {
        building = GetComponent<Building>();
    }

    public void StartConstruction(float buildTime, Action onCompleteCallback)
    {
        if (isConstructing) return;

        isConstructing = true;
        building.SetEnabled(false);
        duration = buildTime;
        timer = 0f;
        onComplete = onCompleteCallback;

        if (defaultVisual != null)
            defaultVisual.SetActive(false);

        if (constructionVisual != null)
            constructionVisual.SetActive(true);
    }

    private void Update()
    {
        if (!isConstructing) return;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            CompleteConstruction();
        }
    }

    private void CompleteConstruction()
    {
        isConstructing = false;
        building.SetEnabled(true);

        if (defaultVisual != null)
            defaultVisual.SetActive(true);

        if (constructionVisual != null)
            constructionVisual.SetActive(false);

        onComplete?.Invoke();
        onComplete = null;
    }
}