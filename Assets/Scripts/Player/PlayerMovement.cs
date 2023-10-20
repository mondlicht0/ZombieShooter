/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    private Vector3 playerVelocity;

    public float PlayerSpeed;
    public float PlayerHeightJump;
    public float gravity = -9.81f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void OnFoot(Vector2 input) 
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        characterController.Move(transform.TransformDirection(moveDirection) * PlayerSpeed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;

        if (playerVelocity.y < 0 && characterController.isGrounded) playerVelocity.y = -2f;

        characterController.Move(playerVelocity * Time.deltaTime);

    }

    public void OnJump()
    {
        if (characterController.isGrounded) playerVelocity.y = Mathf.Sqrt(PlayerHeightJump * -3.0f * gravity);
    }
}*/
