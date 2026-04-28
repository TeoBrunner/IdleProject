using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class InputProvider : MonoBehaviour
{
    public event Action<float> OnMove;
    public event Action OnInteractPressed;
    public event Action OnInteractReleased;
    public event Action OnExaminePressed;
    public event Action OnExamineReleased;
    public bool IsInputEnabled { get; private set; } = true;

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction examineAction;
    private void Awake()
    {
        ServiceLocator.Register<InputProvider>(this);

        PlayerInput playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        interactAction = playerInput.actions["Interact"];
        examineAction = playerInput.actions["Examine"];
    }
    private void OnEnable()
    {
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        interactAction.started += OnInteractPerformed;
        interactAction.canceled += OnInteractCanceled;
        examineAction.started += OnExaminePerformed;
        examineAction.canceled += OnExamineCanceled;
    }
    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
        interactAction.started -= OnInteractPerformed;
        interactAction.canceled -= OnInteractCanceled;
        examineAction.started -= OnExaminePerformed;
        examineAction.canceled -= OnExamineCanceled;
    }
    public void EnableInput(bool enabled)
    {
        IsInputEnabled = enabled;
        if(!enabled)
            OnMove?.Invoke(0f);
    }
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (!IsInputEnabled) return;
        OnMove?.Invoke(context.ReadValue<float>());
    }
    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(0f);
    }
    public void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (!IsInputEnabled) return;
        OnInteractPressed?.Invoke();
    }
    public void OnInteractCanceled(InputAction.CallbackContext context)
    {
        if (!IsInputEnabled) return;
        OnInteractReleased?.Invoke();
    }
    public void OnExaminePerformed(InputAction.CallbackContext context)
    {
        if (!IsInputEnabled) return;
        OnExaminePressed?.Invoke();
    }
    public void OnExamineCanceled(InputAction.CallbackContext context)
    {
        if (!IsInputEnabled) return;
        OnExamineReleased?.Invoke();
    }
}
