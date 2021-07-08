using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEK : MonoBehaviour
{
	[SerializeField] private float speed = 8f;
	[SerializeField] private float gravityMultiplier = 5f;
	[SerializeField] private float gravityCombackMultiplier = 15f;
	[SerializeField] private float maxFallSpeed = 50f;


	private float gravityContributionMultiplier = 0f;
	private bool isJumping;

	private float verticalMovement = 0f;

	private Vector3 inputVector;
	private Vector3 movementVector;

	private CharacterController characterController;

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();

		if(characterController == null)
		{
			Debug.Log("characterController is null");
		}
	}

	private void Update()
	{
		if(!characterController.isGrounded)
		{
			Debug.Log("is not Ground");
			gravityContributionMultiplier += Time.deltaTime * gravityCombackMultiplier;
			gravityContributionMultiplier = Mathf.Clamp01(gravityContributionMultiplier);
			verticalMovement += Physics.gravity.y * gravityMultiplier * Time.deltaTime * gravityCombackMultiplier;

			verticalMovement = Mathf.Clamp(verticalMovement, -maxFallSpeed, 100f);
		}
		else
		{
			// full speed
			movementVector = inputVector * speed;

			Debug.Log("pig move : " + movementVector);

			// Apply the result and move the character in space
			movementVector.y = verticalMovement;
			characterController.Move(movementVector * Time.deltaTime);
		}
		movementVector.y = verticalMovement;
		characterController.Move(movementVector * Time.deltaTime);
	}

	public void Move(Vector3 movement)
	{
		inputVector = movement;
	}
}
