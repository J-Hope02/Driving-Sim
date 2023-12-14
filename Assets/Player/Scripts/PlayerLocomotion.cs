using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotion : MonoBehaviour
{
    ProjectName inputActions;
    InputAction move, look, fire;

    CharacterController characterController;
    public Transform cameraContainer;

    public float maxSpeed = 10f;
    public float jumpSpeed = 10f;
    public float mouseSensitivity = 0.2f;
    public float gravity = 20.0f;

    Vector3 moveDirection = Vector3.zero;
    float speed = 6f;
    float lookUpClamp = -5f;
    float lookDownClamp = 20f;
    float rotateYaw, rotatePitch;
    bool jump = false;

    private void Awake()
    {
        inputActions = new ProjectName();
    }

    void Start()
    {
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();

        inputActions.Player.Move.Enable();
        inputActions.Player.Look.Enable();
        inputActions.Player.Fire.Enable();
    }

    void Update()
    {
        RotateAndLook();
    }

    void FixedUpdate()
    {
        if (move.ReadValue<Vector2>() != Vector2.zero)
        {
            Debug.Log("Move: " + move.ReadValue<Vector2>());
        }
        if (look.ReadValue<Vector2>() != Vector2.zero)
        {
            Debug.Log("Look: " + look.ReadValue<Vector2>());
        }

        Locomotion();
    }

    private void OnEnable()
    {
        move = inputActions.Player.Move;
        move.Enable();

        look = inputActions.Player.Look;
        look.Enable();

        fire = inputActions.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        fire.Disable();
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire Button Pressed");
        jump = true;
    }

    void Locomotion()
    {
        if (characterController.isGrounded) // When grounded, set y-axis to zero (to ignore it)
        {
            moveDirection = new Vector3(move.ReadValue<Vector2>().x, 0f, move.ReadValue<Vector2>().y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (jump)
            {
                jump = false;
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void RotateAndLook()
    {
        Vector2 lookInput = look.ReadValue<Vector2>();

        rotateYaw = lookInput.x * mouseSensitivity;
        rotateYaw += cameraContainer.transform.localRotation.eulerAngles.y;

        rotatePitch -= lookInput.y * mouseSensitivity;
        rotatePitch = Mathf.Clamp(rotatePitch, lookUpClamp, lookDownClamp);

        cameraContainer.transform.localRotation = Quaternion.Euler(rotatePitch, rotateYaw, 0f);
    }
}