using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
	[SerializeField] private float _minDistance = 0.2f;
	[SerializeField] private float _maxTime = 1f;
	[SerializeField, Range(0, 1)] private float _directionThreshold = 0.9f;

	private InputHandler _inputHandler;

	private Vector2 _startPosition;
	private float _startTime;

	private Vector2 _endPosition;
	private float _endTime;

	private bool _up, _down, _left, _right;

	public bool ToUp { get => _up; }
	public bool ToDown { get => _down; }
	public bool ToLeft { get => _left; }
	public bool ToRight { get => _right; }

	private void Awake()
	{
		_inputHandler = GetComponent<InputHandler>();
	}

	private void OnEnable()
	{
		_inputHandler.OnStartTouch += SwipeStart;
		_inputHandler.OnEndTouch += SwipeEnd;
	}

	private void OnDisable()
	{
		_inputHandler.OnStartTouch -= SwipeStart;
		_inputHandler.OnEndTouch -= SwipeEnd;
	}

	private void SwipeStart(Vector2 position, float time)
	{
		_startPosition = position;
		_startTime = time;
	}

	private void SwipeEnd(Vector2 position, float time)
	{
		_endPosition = position;
		_endTime = time;

		DetectSwipe();
	}

	private void DetectSwipe()
	{
		if (Vector3.Distance(_startPosition, _endPosition) >= _minDistance && (_endTime - _startTime) <= _maxTime)
		{
			Debug.Log("Up");
			Vector3 direction = _endPosition - _startPosition;
			Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
			SwipeDirection(direction2D);
		}
	}

	private void SwipeDirection(Vector2 direction)
	{
		if (Vector2.Dot(Vector2.up, direction) > _directionThreshold)
		{
			Debug.Log("Up");

			_up = true;
			_down = false;
			_right = false;
			_left = false;
		}

		else if (Vector2.Dot(Vector2.down, direction) > _directionThreshold)
		{
			Debug.Log("Down");

			_up = false;
			_down = true;
			_right = false;
			_left = false;
		}

		else if(Vector2.Dot(Vector2.right, direction) > _directionThreshold)
		{
			Debug.Log("Right");

			_up = false;
			_down = false;
			_right = true;
			_left = false;
		}

		else if(Vector2.Dot(Vector2.left, direction) > _directionThreshold)
		{
			Debug.Log("Left");

			_up = false;
			_down = false;
			_right = false;
			_left = true;
		}
	}
}
