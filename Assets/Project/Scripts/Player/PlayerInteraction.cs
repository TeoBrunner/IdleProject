using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private InputProvider inputProvider;

    public IInteractable CurrentInteractable { get; private set; }
    public bool hasInteractable => CurrentInteractable != null;

    private void Start()
    {
        inputProvider = ServiceLocator.Get<InputProvider>();
        if (inputProvider)
        {
            inputProvider.OnInteractPressed += HandleInteractPressed;
            inputProvider.OnExaminePressed += HandleExaminePressed;
        }
            
    }
    private void OnEnable()
    {
        if (inputProvider)
        {
            inputProvider.OnInteractPressed += HandleInteractPressed;
            inputProvider.OnExaminePressed += HandleExaminePressed;
        }
            
    }
        
    private void OnDisable()
    {
        if (inputProvider)
        {
            inputProvider.OnInteractPressed -= HandleInteractPressed;
            inputProvider.OnExaminePressed -= HandleExaminePressed;
        }
            
    }
    private void HandleInteractPressed()
    {
        CurrentInteractable?.OnInteract();
    } 
    private void HandleExaminePressed()
    {
        CurrentInteractable?.OnExamine();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IInteractable>(out var interactable)) return;

        CurrentInteractable?.OnPlayerExit();
        CurrentInteractable = interactable;
        CurrentInteractable.OnPlayerEnter();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<IInteractable>(out var interactable)) return;
        if (CurrentInteractable != interactable) return;

        CurrentInteractable.OnPlayerExit();
        CurrentInteractable = null;
    }
}
