using UnityEngine;

public abstract class PlayerBaseState
{
    private Player _player;
    private PlayerStateMachine _context;
    private PlayerStateFactory _factory;

    protected PlayerBaseState _currentSubState;
    protected PlayerBaseState _currentSuperState;

    private bool _isRootState = false;

    public Player Player { get { return _player; } }
    public PlayerStateMachine Context { get { return _context; } }
    public PlayerStateFactory Factory { get { return _factory; } }
    public bool IsRootState { get { return _isRootState; } set { _isRootState = value; } }

    public PlayerBaseState(Player player, PlayerStateMachine context, PlayerData playerData, PlayerStateFactory playerStateFactory)
    {
        this._context = context;
        this._factory = playerStateFactory;
        this._player = player;
        this._player.PlayerData = playerData;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void CheckChangeStates();
    public abstract void InitializeSubState();
    public abstract void AnimationTriggerEvent();

    public void UpdateStates() 
    { 
        LogicUpdate();
        PhysicsUpdate();

        if (_currentSubState != null) { _currentSubState.LogicUpdate(); _currentSubState.PhysicsUpdate(); }
    }

    protected void ChangeState(PlayerBaseState newState) 
    {
        Exit();

        newState.Enter();

        if (_isRootState) _context.CurrentState = newState;

        else if (_currentSuperState != null) _currentSuperState.SetSubState(newState);
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

}
