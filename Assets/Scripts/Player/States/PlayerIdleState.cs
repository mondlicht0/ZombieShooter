using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(Player player, PlayerStateMachine context, PlayerData playerData, PlayerStateFactory playerStateFactory) : base(player, context, playerData, playerStateFactory)
    {
        
    }

    public void HandleGravity()
    {
        Player.PlayerVelocity.y += Player.PlayerData.GroundedGravity * Time.deltaTime;

        if (Player.PlayerVelocity.y < 0 && Player.Controller.isGrounded) Player.PlayerVelocity.y = -2f;

        Player.Controller.Move(Player.PlayerVelocity * Time.deltaTime);
    }

    public override void Enter()
    {
        Debug.Log("Enter to Idle");

        Context.Armature.SetBool("IsWalking", false);

        Player.PlayerVelocity.x = 0;
        Player.PlayerVelocity.z = 0;

        Player.HeadBob.SetIsWalking(false);

    }

    public override void Exit()
    {

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

    }

    public override void CheckChangeStates()
    {
        if (Context.InputHandler.IsMovementPressed) ChangeState(Factory.Walk());

        else if (Context.InputHandler.IsShiftPressed) ChangeState(Factory.Dash());
    }

    public override void AnimationTriggerEvent()
    {
        
    }
}
