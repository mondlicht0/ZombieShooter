using UnityEngine;
using UnityEngine.Windows;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(Player player, PlayerStateMachine context, PlayerData playerData, PlayerStateFactory playerStateFactory) : base(player, context, playerData, playerStateFactory)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter to Run");

        Player.Camera.m_Lens.FieldOfView = Mathf.Lerp(70, 90, 1f);
    }

    public override void Exit()
    {
        
    }

    public override void LogicUpdate()
    {
        CheckChangeStates();

        Run();
    }

    public override void PhysicsUpdate()
    {

    }

    public override void CheckChangeStates()
    {
        if (!Context.InputHandler.IsMovementPressed) ChangeState(Factory.Idle());

        else if (Context.InputHandler.IsMovementPressed && !Context.InputHandler.IsShiftPressed) ChangeState(Factory.Walk());
    }
    public override void InitializeSubState()
    {

    }

    public override void AnimationTriggerEvent()
    {
        
    }
    private void Run()
    {
        Player.MoveSpeed = Player.PlayerData.RunSpeed;
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(Player.Rigidbody.velocity.x, 0f, Player.Rigidbody.velocity.z);

        if (flatVelocity.magnitude > Player.PlayerData.RunSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * Player.PlayerData.RunSpeed;
            Player.Rigidbody.velocity = new Vector3(limitedVelocity.x, Player.Rigidbody.velocity.y, limitedVelocity.z);
        }
    }
}
