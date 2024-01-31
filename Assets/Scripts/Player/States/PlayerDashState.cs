using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState, IRootState
{
    private Vector3 dashDirection;
    private Vector3 _delayedForceToApply;

    public PlayerDashState(Player player, PlayerStateMachine context, PlayerData playerData, PlayerStateFactory playerStateFactory) : base(player, context, playerData, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void Enter()
    {
        Debug.Log("Enter to Dashing");

        dashDirection = Player.Orientation.forward;
        dashDirection.Normalize();

        Dash();
    }

    public override void Exit()
    {
        
    }

    public void HandleGravity()
    {
        Player.PlayerVelocity.y += Player.PlayerData.GroundedGravity * Time.deltaTime;
    }

    public override void LogicUpdate()
    {
        CheckChangeStates();

        if (Player.DashCDTimer > 0) Player.DashCDTimer -= Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        HandleGravity();

        CheckGround();
    }

    public override void CheckChangeStates()
    {
        if (Player.IsGrounded) Context.ChangeState(Factory.Grounded());
    }


    public override void InitializeSubState()
    {

    }

    public override void AnimationTriggerEvent()
    {

    }

    private void CheckGround()
    {
        Player.IsGrounded = Physics.Raycast(Player.transform.position, Vector3.down, Player.PlayerData.PlayerHeight * 0.5f + 0.2f, Player.PlayerData.GroundLayer);

        if (Player.IsGrounded) Player.Rigidbody.drag = Player.PlayerData.GroundDrag;
        else Player.Rigidbody.drag = 0;
    }

    private void Dash()
    {
        if (Player.DashCDTimer > 0) return;
        else Player.DashCDTimer = Player.PlayerData.DashCD;

        Player.IsDashing = false;

        //Context.InvokeDashing();

        //Exit();
    }



    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (Player.PlayerData.AllowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}
