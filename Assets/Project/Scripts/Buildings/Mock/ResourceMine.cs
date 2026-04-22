using UnityEngine;

public class ResourceMine : MonoBehaviour, IInteractable
{
    [SerializeField] ResourceDefinition resourceDefinition;
    [SerializeField] int resourcePerInteraction = 1;

    private ResourceManager resourceManager;
    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
    }
    public void OnInteract()
    {
        resourceManager.Add(resourceDefinition, resourcePerInteraction);
    }

    public void OnPlayerEnter()
    {
        
    }

    public void OnPlayerExit()
    {
        
    }

}
