using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Vector2 movementDirection;
    [SerializeField] Rigidbody rb;
    [Space]
    [SerializeField] Transform horizontalMove;
    [SerializeField] Transform verticalMove;
    [Header("Vars")]
    [SerializeField] float current_speed;
    [SerializeField] float walk_speed = 5f;
    [SerializeField] float run_speed= 10f;
    [Space]
    [SerializeField] float sensitivity = 1f;
    float verticalRotation;

    public bool stopMovementView;
    public bool stopMovementWalk;

    private void Awake()
    {
        playerInput.OnPlayerMove += OnMove;
        playerInput.OnPlayerStop += OnStop;
        playerInput.OnMouseMovementEvent += OnMouseMovement;
        playerInput.OnSprintStart += OnStartRun;
        playerInput.OnSprintStop += OnStopRun;
    }
    private void Start()
    {
        current_speed = walk_speed;
    }
    private void FixedUpdate()
    {
        Vector3 moveDirection = horizontalMove.right * movementDirection.x + horizontalMove.forward * movementDirection.y;

        moveDirection.y = 0f;

        if (moveDirection.sqrMagnitude > 1f)
            moveDirection.Normalize();

        rb.linearVelocity = new Vector3(moveDirection.x * current_speed, rb.linearVelocity.y, moveDirection.z * current_speed);
    }
    void OnMove()
    {
        if (stopMovementWalk)
            return;
        movementDirection = new Vector2(
        playerInput.MovementX,
        playerInput.MovementY
    );
    }
    public void OnStop()
    {
        movementDirection = Vector2.zero;
    }
    void OnMouseMovement()
    {
        if (stopMovementView)
            return;
        float mouseX = playerInput.MouseX;
        float mouseY = playerInput.MouseY;

        // Rotazione orizzontale: solo Y
        horizontalMove.Rotate(0f, mouseX * sensitivity, 0f);

        // Accumula la rotazione verticale
        verticalRotation -= mouseY * sensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -85f, 85f);

        // Rotazione verticale: solo X
        verticalMove.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
    void OnStartRun()
    {
        current_speed = run_speed;
    }
    void OnStopRun()
    {
        current_speed = walk_speed;
    }
}
