using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistEK : MonoBehaviour
{
	public InputReaderEK inputReaderEK;

	private CharacterEK characterEK;
	private Vector2 previousMovementInput;
	private bool enableControl = true;

	private void Awake()
	{
		characterEK = GetComponent<CharacterEK>();
	}

	private void OnEnable()
	{
		inputReaderEK.moveEvent += OnMoveEvent;
		inputReaderEK.jumpEvent += OnJumpEvent;
		inputReaderEK.jumpCanceledEvent += OnJumpCanceled;
	}

	private void OnDisable()
	{
		inputReaderEK.moveEvent -= OnMoveEvent;
		inputReaderEK.jumpEvent -= OnJumpEvent;
		inputReaderEK.jumpCanceledEvent -= OnJumpCanceled;
	}

	private void OnJumpEvent()
	{
		if(enableControl)
		{
			Debug.Log("Jump!!");
			characterEK.Jump();
		}
	}

	private void OnMoveEvent(Vector2 arg0)
	{
		if(enableControl)
		{
			Debug.Log("Move!!");
			previousMovementInput = arg0;
		}
	}

	private void OnJumpCanceled()
	{
		if (enableControl)
			characterEK.CancelJump();
	}

	// Update is called once per frame
	void Update()
    {
		RecalculateMovement();
    }

	private void RecalculateMovement()
	{
		Vector3 adustedMovement = Vector3.right * previousMovementInput.x +
			Vector3.forward * previousMovementInput.y;

		//Debug.Log("adjusted v3 : " + adustedMovement);

		characterEK.Move(Vector3.ClampMagnitude(adustedMovement, 1f));
	}
}
