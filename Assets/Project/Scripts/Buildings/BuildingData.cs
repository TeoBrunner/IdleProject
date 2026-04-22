using UnityEngine;

[CreateAssetMenu(menuName = "Buildings/Building Data", fileName = "BuildingData")]
public class BuildingData : ScriptableObject
{
    [Header("Main")]
    [SerializeField] private string buildingId;
    [SerializeField] private string displayName;
    [SerializeField, TextArea(2, 4)] private string description;
    [SerializeField] private ResourceDefinition producedResource;

    [Header("Manual Clicks")]
    [SerializeField] private bool hasManualClick;
    [SerializeField] private int resourcePerClick = 1;

    [Header("Auto Clicks")]
    [SerializeField] private bool hasAutoClick;
    [SerializeField] private int resourcePerAutoClick = 1;
    [SerializeField] private float tickInterval = 1f;

    public string BuildingId => buildingId;
    public string DisplayName => displayName;
    public string Description => description;
    public ResourceDefinition ProducedResource => producedResource;
    public bool HasManualClick => hasManualClick;
    public int ResourcePerClick => resourcePerClick;
    public bool HasAutoClick => hasAutoClick;
    public int ResourcePerAutoClick => resourcePerAutoClick;
    public float TickInterval => tickInterval;
}
