using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Input ReaderEK", menuName = "Game/Input ReaderEK")]
public class InputReaderEK : ScriptableObject, GameInputEK.IPlayerActions
{
	public UnityAction<Vector2> moveEvent;
	public UnityAction jumpEvent;
	public UnityAction jumpCanceledEvent;

	private GameInputEK gameInputEK;

	private void OnEnable()
	{
		Debug.Log("InputReader Enabled!!");
		if(gameInputEK == null)
		{
			gameInputEK = new GameInputEK();
			gameInputEK.Player.SetCallbacks(this);
		}
		gameInputEK.Enable();
	}

	private void OnDisable()
	{
		gameInputEK.Disable();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		Debug.Log("!!");
		if(jumpEvent != null && context.phase == InputActionPhase.Started)
		{
			Debug.Log("Jump Event Called!!");
			jumpEvent.Invoke();
		}

		if (jumpCanceledEvent != null && context.phase == InputActionPhase.Canceled)
			jumpCanceledEvent.Invoke();
	}

	public void OnMovement(InputAction.CallbackContext context)
	{
		if(moveEvent != null)
		{
			moveEvent.Invoke(context.ReadValue<Vector2>());
		}
	}
}
