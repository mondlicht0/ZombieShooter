using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    private RaycastHit _slopeHit;

    public PlayerGroundedState(Player player, PlayerStateMachine context, PlayerData playerData, PlayerStateFactory playerStateFactory) : base(player, context, playerData, playerStateFactory)
    {
        IsRootState = true;
    }

    public void HandleGravity()
    {
        Player.PlayerVelocity.y += Player.PlayerData.GroundedGravity * Time.deltaTime;

        if (Player.PlayerVelocity.y < 0 && Player.Controller.isGrounded) Player.PlayerVelocity.y = -2f;

        Player.Controller.Move(Player.PlayerVelocity * Time.deltaTime);
    }

    public override void Enter()
    {
        Debug.Log("Enter to Grounded"); 
        InitializeSubState();
        //HandleGravity();
    }

    public override void Exit()
    {
        Debug.Log("Exit from Grounded");
    }

    public override void LogicUpdate()
    {
        CheckChangeStates();
    }

    public override void PhysicsUpdate()
    {
        HandleGravity();
    }

    public override void InitializeSubState()
    {
        if (!Context.InputHandler.IsMovementPressed) SetSubState(Factory.Idle());

        else if (Context.InputHandler.IsMovementPressed) SetSubState(Factory.Walk());

        //else SetSubState(Factory.Run());
    }

    public override void CheckChangeStates()
    {
        if (Context.InputHandler.JumpInput && Player.IsGrounded && Player.IsReadyToJump) ChangeState(Factory.Jump());

        else if (Context.InputHandler.IsShiftPressed)
            ChangeState(Factory.Dash());

        //else if (!Player.CharacterController.isGrounded) ChangeState(Factory.Fall());
    }

    public override void AnimationTriggerEvent()
    {
        
    }
}
