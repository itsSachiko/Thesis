using DesignPatterns.Generics;
using Subtegral.DialogueSystem.DataContainers;
using Subtegral.DialogueSystem.Runtime;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject UIPanelsContainer;
    [SerializeField] GameObject normalPointerImage;
    [SerializeField] GameObject interactPointerImage;
    [SerializeField] DialogueParser DialogueManager;
    public override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        ShowNormalPointer();
    }
    public void ShowNormalPointer()
    {
        normalPointerImage.SetActive(true);
        interactPointerImage.SetActive(false);
    }
    public void ShowInteractPointer()
    {
        normalPointerImage.SetActive(false);
        interactPointerImage.SetActive(true);
    }
    public void StartDialogue(DialogueContainer _dialogue)
    {
        DialogueManager.gameObject.SetActive(true);
        DialogueManager.StartDialogue(_dialogue);

        GameManager.Instance.playerController.stopMovementView = true;
        GameManager.Instance.playerController.stopMovementWalk = true;
        GameManager.Instance.playerController.OnStop();
        GameManager.Instance.UnhideNUnlockMouse();
        GameManager.Instance.playerInteracter.canInteract = false;
    }
    public void EndDialogue()
    {
        DialogueManager.EndDialogue();

        GameManager.Instance.playerController.stopMovementView = false;
        GameManager.Instance.playerController.stopMovementWalk = false;
        GameManager.Instance.HideNLockMouse();
        GameManager.Instance.playerInteracter.canInteract = true;
    }
}
