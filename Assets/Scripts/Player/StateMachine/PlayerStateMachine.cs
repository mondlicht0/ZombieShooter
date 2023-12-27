using System.Collections;
using UnityEngine;
using Cinemachine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private NoiseSettings _noiseSettings;
    [SerializeField] private Animator _armatureAnimator;

    private PlayerStates currState;

    private Player _player;
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    private InputHandler _inputHandler;
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Player Player { get { return _player; } }
    public InputHandler InputHandler { get { return _inputHandler; } }
    public NoiseSettings NoiseSettings { get { return _noiseSettings; } }
    public Animator Armature { get { return _armatureAnimator; } }


    private void Awake()
    {
        _player = GetComponent<Player>();
        _inputHandler = GetComponent<InputHandler>();
        _states = new PlayerStateFactory(this);

        Initialize(_states.Grounded());
    }
    private void Update()
    {
        _currentState.UpdateStates();
    }

    public void Initialize(PlayerBaseState startingState)
    {
        _currentState = startingState;
        _currentState.Enter();
    }

    public void ChangeState(PlayerBaseState state)
    {
        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    public void TranslateToNewValues(float frequency, float amplitude)
    {
        //_noiseSettings.PositionNoise[0].Y.Frequency = Mathf.Lerp(_noiseSettings.PositionNoise[0].Y.Frequency, frequency, 2f);
        //_noiseSettings.PositionNoise[0].Y.Amplitude = Mathf.Lerp(_noiseSettings.PositionNoise[0].Y.Amplitude, amplitude, 2f);

        _noiseSettings.PositionNoise[0].Y.Frequency = frequency;
        _noiseSettings.PositionNoise[0].Y.Amplitude = amplitude;
    }
}
