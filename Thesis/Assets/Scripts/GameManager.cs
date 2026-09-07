using DesignPatterns.Generics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerInput playerInput;
    public Interacter playerInteracter;
    public PlayerController playerController;
    [SerializeField] bool startWithMouseHide;
    [SerializeField] bool startWithMouseOnCenter;
    public override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        if (startWithMouseHide)
            Cursor.visible = false;
        if(startWithMouseOnCenter)
            Cursor.lockState = CursorLockMode.Locked;
    }
    public void HideNLockMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void UnhideNUnlockMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
