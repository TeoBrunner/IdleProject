using UnityEngine;

[CreateAssetMenu(menuName = "Buildings/Building Data", fileName = "BuildingData")]
public class BuildingData : ScriptableObject
{
    [Header("Main")]
    [SerializeField] private string buildingId;
    [SerializeField] private string displayName;
    [SerializeField, TextArea(2, 4)] private string description;

    [Header("Auto Clicks")]
    [SerializeField] private bool hasAutoClick;
    [SerializeField] private ResourceDefinition producedResource;
    [SerializeField] private int resourcePerTick = 1;
    [SerializeField] private float tickInterval = 1f;

    public string BuildingId => buildingId;
    public string DisplayName => displayName;
    public string Description => description;
    public bool HasAutoClick => hasAutoClick;
    public ResourceDefinition ProducedResource => producedResource;
    public int ResourcePerTick => resourcePerTick;
    public float TickInterval => tickInterval;
}
