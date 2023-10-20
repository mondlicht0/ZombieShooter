using System.Collections.Generic;

enum PlayerStates
{
    Idle,
    Walk,
    Run,
    Jump,
    Grounded,
    Fall,
    Dash
}

public class PlayerStateFactory 
{
    private PlayerStateMachine _context;

    private Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory (PlayerStateMachine context)
    {
        _context = context;
        _states[PlayerStates.Idle] = new PlayerIdleState(_context.Player, _context, _context.Player.PlayerData, this);
        _states[PlayerStates.Walk] = new PlayerWalkState(_context.Player, _context, _context.Player.PlayerData, this);
        _states[PlayerStates.Run] = new PlayerRunState(_context.Player, _context, _context.Player.PlayerData, this);
        _states[PlayerStates.Jump] = new PlayerJumpState(_context.Player, _context, _context.Player.PlayerData, this);
        _states[PlayerStates.Grounded] = new PlayerGroundedState(_context.Player, _context, _context.Player.PlayerData, this);
        _states[PlayerStates.Fall] = new PlayerFallState(_context.Player, _context, _context.Player.PlayerData, this);
        _states[PlayerStates.Dash] = new PlayerDashState(_context.Player, _context, _context.Player.PlayerData, this);
    }

    public PlayerBaseState Idle() { return _states[PlayerStates.Idle]; }
    public PlayerBaseState Walk() { return _states[PlayerStates.Walk]; }
    public PlayerBaseState Run() { return _states[PlayerStates.Run]; }
    public PlayerBaseState Jump() { return _states[PlayerStates.Jump]; }
    public PlayerBaseState Grounded() { return _states[PlayerStates.Grounded]; }
    public PlayerBaseState Fall() { return _states[PlayerStates.Fall]; }
    public PlayerBaseState Dash() { return _states[PlayerStates.Dash]; }
    }
