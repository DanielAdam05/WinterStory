using UnityEngine;
using UnityEngine.InputSystem;

// This component is automatically added when the PlayerController script is attached.
[RequireComponent(typeof(CharacterController))]
public class WalkingController : MonoBehaviour
{
    [Header("Transform References")]
    [SerializeField]
    private Transform cameraTransform;

    [Space(8)]
    [SerializeField]
    private float walkingMoveSpeed = 6f;
    [SerializeField]
    private float verticalVelocity;
    [SerializeField]
    private float constantJumpVelocity = 5f;

    [Header("Input Action References")]
    [SerializeField]
    public InputActionReference playerMoveActionReference;
    [SerializeField]
    private InputActionReference jumpActionReference;

    // Non-assignable variables
    private CharacterController characterController;
    private Vector2 moveInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerMoveActionReference.action.Enable();
        characterController.detectCollisions = true;
        characterController.enabled = true; // Ensure controller is enabled on walking start
    }

    private void OnDisable()
    {
        if (this.gameObject.activeSelf == true)
        {
            playerMoveActionReference.action.Disable();
            characterController.enabled = false; // Ensure controller is enabled on walking start
        }
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveInput = playerMoveActionReference.action.ReadValue<Vector2>();

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 desiredMove = camForward * moveInput.y + camRight * moveInput.x;
        desiredMove = desiredMove.normalized;

        ApplyGravity(-20f);

        if (characterController.isGrounded)
        {
            Jump();
        }

        Vector3 playerMovement = walkingMoveSpeed * Time.deltaTime * desiredMove;
        playerMovement.y = verticalVelocity * Time.deltaTime;

        characterController.Move(playerMovement);
    }

    private void Jump()
    {
        if (jumpActionReference.action.triggered)
        {
            verticalVelocity = constantJumpVelocity;
        }
    }

    private void ApplyGravity(float gravity)
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -1f;
        }
    }

    public void TeleportPlayer(Vector3 pos)
    {
        transform.parent = null;
        characterController.enabled = false;
        transform.position = pos;
        characterController.enabled = true;
    }
}
