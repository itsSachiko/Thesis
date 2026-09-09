using System;
using UnityEngine;

/// <summary>
/// this class is responsible for handling player interactions with interactable objects in the game world. 
/// It uses raycasting to detect interactable objects in front of the player and triggers their interaction logic when the player presses the interaction button.
/// </summary>
public class Interacter : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Camera playerCamera;
    [SerializeField] float interactionDistance = 3f;

    [SerializeField] bool isWatchingInteractable;
    [SerializeField] bool wasWatching;

    public bool canInteract = true;
    private void Awake()
    {
        playerInput.OnInteraction += TryInteract;
    }
    private void Update()
    {
        isWatchingInteractable = false;

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out RaycastHit hit,
            interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            isWatchingInteractable = interactable != null;
        }
        if (wasWatching == isWatchingInteractable)
            return;

        UpdatePointerUI(isWatchingInteractable);
        wasWatching = isWatchingInteractable;
    }
    void UpdatePointerUI(bool _isWatchingInteractable)
    {
        if (_isWatchingInteractable)
        {
            UIManager.Instance.ShowInteractPointer();
        }
        else
        {
            UIManager.Instance.ShowNormalPointer();
        }
    }
    void TryInteract()
    {
        if (!canInteract)
            return;

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out RaycastHit hit,
            interactionDistance))
        {
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact(gameObject);
            }
        }
    }
}
