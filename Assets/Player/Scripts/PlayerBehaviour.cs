using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    ProjectName inputActions;

    InputAction turn, brakePedal, accelerate, test;
    CharacterController characterController;

    public float maxSpeed = 10f;
    float speed = 0f;
    float accelerationMultiplier = 0.2f;
    float defaultDrag = 0.01f;
    float brakeDrag = 0.02f;
    float drag = 0.15f;

    float brakespeed = 0f;

    public float gravity = 20.0f;
    Vector3 moveDirection = Vector3.zero;
    private void Awake()
    {
        inputActions = new ProjectName();
    }

    void Start()
    {
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();

        inputActions.Drive.Turn.Enable();
        inputActions.Drive.BrakePedal.Enable();
        inputActions.Drive.Accelerate.Enable();
/*        inputActions.Drive.Test.Enable();*/
    }

    void FixedUpdate()
    {
        Debug.Log("Turn: " + turn.ReadValue<Vector2>().x);
        Debug.Log("Accelerate: " + accelerate.ReadValue<float>());
        Debug.Log("Brake: " + brakePedal.ReadValue<float>());

       /* Debug.Log("TEST: " + test.ReadValue<float>());*/

        Locomotion();
    }

    private void OnEnable()
    {
        turn = inputActions.Drive.Turn;
        turn.Enable();

        accelerate = inputActions.Drive.Accelerate;
        accelerate.Enable();

        brakePedal = inputActions.Drive.BrakePedal;
        brakePedal.Enable();

/*        test = inputActions.Drive.Test;
        test.Enable();*/
    }

    private void OnDisable()
    {
        turn.Disable();
        accelerate.Disable();
        brakePedal.Disable();
    }

    void Locomotion()
    {
        if (characterController.isGrounded) // When grounded, set y-axis to zero (to ignore it)
        {
            float acceleration = accelerate.ReadValue<float>();
            float breaking = brakePedal.ReadValue<float>();
            float turning = turn.ReadValue<Vector2>().x;

            drag = 1 - defaultDrag - (brakeDrag * breaking);

            speed += acceleration * accelerationMultiplier;
            speed *= drag;

            if (speed <= 0.1)
            {
                speed = 0;
            }
            else if (speed >= maxSpeed)
            {
                speed = maxSpeed;
            }

            moveDirection = new Vector3(0f, 0f, speed);
            moveDirection = transform.TransformDirection(moveDirection);

            turning *= speed;
            turning = Mathf.Clamp(turning, -5f, +5f);
            transform.Rotate(0f, turning, 0f);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}