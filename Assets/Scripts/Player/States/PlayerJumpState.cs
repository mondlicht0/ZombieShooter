using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    public PlayerJumpState(Player player, PlayerStateMachine context, PlayerData playerData, PlayerStateFactory playerStateFactory) : base(player, context, playerData, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void Enter()
    {
        Debug.Log("Enter to Jump");

        InitializeSubState();

        Jump();
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
        if (Player.Controller.isGrounded) ChangeState(Factory.Grounded());
    }

    public void Jump()
    {
        Player.PlayerVelocity.y = Mathf.Sqrt(Player.PlayerData.JumpHeight * -3.0f * Player.PlayerData.Gravity);
    }

    public void HandleGravity()
    {
        Player.PlayerVelocity.y += Player.PlayerData.Gravity * Time.deltaTime;
        Player.Controller.Move(Player.PlayerVelocity * Time.deltaTime);
    }

    public override void AnimationTriggerEvent()
    {

    }

}
