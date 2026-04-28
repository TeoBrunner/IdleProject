using UnityEngine;

public interface IInteractable
{
    void OnPlayerEnter();
    void OnPlayerExit();
    void OnInteract();
    void OnExamine();

}
