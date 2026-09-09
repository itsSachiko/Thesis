using DesignPatterns.Generics;
using UnityEngine;

/// <summary>
/// GameManager is a singleton class that manages the overall game state and player interactions. 
/// It holds references to the PlayerInput, Interacter, and PlayerController components, allowing centralized control over player input and interactions.
/// The class also provides functionality to hide and lock the mouse cursor, as well as to unhide and unlock it, based on the specified settings at the start of the game.
/// </summary>
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
