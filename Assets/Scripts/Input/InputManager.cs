/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnMoveActions moveActions;

    private PlayerMovement playerMotor;

    private Vector2 movementInput;

    private void Awake()
    {
        playerInput = new PlayerInput();

        playerMotor = GetComponent<PlayerMovement>();

        moveActions = playerInput.OnMove;

        moveActions.Jump.performed += ctx => playerMotor.OnJump();
    }

    private void FixedUpdate()
    {
        playerMotor.OnFoot(moveActions.Movement.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        moveActions.Enable();
    }

    private void OnDisable()
    {
        moveActions.Disable();
    }


}
*/