using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootState
{
    public PlayerFallState(Player player, PlayerStateMachine context, PlayerData playerData, PlayerStateFactory playerStateFactory) : base(player, context, playerData, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void Enter()
    {
        Debug.Log("Enter to Fall");

        InitializeSubState();
    }

    public override void Exit()
    {
        Debug.Log("Exit from Fall");
    }
    public override void LogicUpdate()
    {
        CheckChangeStates();
    }

    public override void PhysicsUpdate()
    {
        HandleGravity();
    }

    public override void CheckChangeStates()
    {
        //if (Player.CharacterController.isGrounded) ChangeState(Factory.Grounded());
    }
    public override void InitializeSubState()
    {

    }

    public void HandleGravity()
    {

    }

    public override void AnimationTriggerEvent()
    {
        
    }
}
