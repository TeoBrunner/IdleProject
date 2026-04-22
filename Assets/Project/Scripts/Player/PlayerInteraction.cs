using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private InputReader inputReader;

    public IInteractable CurrentInteractable { get; private set; }
    public bool hasInteractable => CurrentInteractable != null;

    private void Awake()
    {
        inputReader = ServiceLocator.Get<InputReader>();
    }
    private void OnEnable() => inputReader.OnInteractPressed += HandleInteractPressed;
    private void OnDisable() => inputReader.OnInteractPressed -= HandleInteractPressed;

    private void HandleInteractPressed()
    {
        CurrentInteractable?.OnInteract();
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
