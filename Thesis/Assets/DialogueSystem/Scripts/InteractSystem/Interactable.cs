using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent onInteracted;
    public void Interact(GameObject interactor)
    {
        onInteracted?.Invoke();
    }
}
