using UnityEngine;

[CreateAssetMenu(menuName = "Resources/Resource Definition", fileName = "ResourceDefinition")]
public class ResourceDefinition : ScriptableObject
{
    [SerializeField] string resourceId;
    [SerializeField] string displayName;
    [SerializeField] Sprite icon;

    public string ResourceId => resourceId;

    public string DisplayName => displayName;

    public Sprite Icon => icon;
}
