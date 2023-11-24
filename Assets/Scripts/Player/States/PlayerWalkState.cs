using Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(Player player, PlayerStateMachine context, PlayerData playerData, PlayerStateFactory playerStateFactory) : base(player, context, playerData, playerStateFactory)
    {
        
    }


    public override void Enter()
    {
        //Debug.Log("Enter to Walk");

        Context.Armature.SetBool("IsWalking", true);
        Player.HeadBob.SetIsWalking(true);
    }

    public override void Exit()
    {
        //Debug.Log("Exit from Walk");
    }

    public override void LogicUpdate()
    {
        CheckChangeStates();
        Walk(Context.InputHandler.MovementInput);
    }

    public override void PhysicsUpdate()
    {

    }

    public override void CheckChangeStates()
    {
        if (!Context.InputHandler.IsMovementPressed) ChangeState(Factory.Idle());

        else if (Context.InputHandler.IsShiftPressed) ChangeState(Factory.Dash());

        //else if (Context.InputHandler.IsMovementPressed && Context.InputHandler.IsShiftPressed) ChangeState(Factory.Run());
    }

    public override void InitializeSubState()
    {

    }

    public void Walk(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        Player.Controller.Move(Player.transform.TransformDirection(moveDirection) * Player.PlayerData.WalkSpeed * Time.deltaTime);

        Player.PlayerVelocity.y += Player.PlayerData.Gravity * Time.deltaTime;

        if (Player.PlayerVelocity.y < 0 && Player.Controller.isGrounded) Player.PlayerVelocity.y = -2f;

        Player.Controller.Move(Player.PlayerVelocity * Time.deltaTime);
    }

    public override void AnimationTriggerEvent()
    {
        
    }

}
