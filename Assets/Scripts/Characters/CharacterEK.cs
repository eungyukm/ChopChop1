using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEK : MonoBehaviour
{
	private CharacterController characterController;

	[SerializeField] private float speed = 8f;
	[SerializeField] private float gravityMultiplier = 5f;
	[SerializeField] private float gravityCombackMultiplier = 15f;
	[SerializeField] private float maxFallSpeed = 50f;
	[SerializeField] private float initialJumpForce = 10;
	[SerializeField] private float jumpInputDuration = 0.4f;
	[SerializeField] private float gravityDivider = 0.6f;
	[SerializeField] private float fallingVerticalMovement = -5f;
	[SerializeField] private float turnSmoothTime = 0.2f;
	[SerializeField] private float slideFriction = 0.3f;


	private float gravityContributionMultiplier = 0f;
	private bool isJumping;
	private float jumpBeginTime;
	private float turnSmoothSpeed;
	private float verticalMovement = 0f;
	private Vector3 hitNormal;

	private Vector3 inputVector;
	private Vector3 movementVector;
	private float currentSlope;
	private bool shouldSlide;
	private const float ROTATION_THRESHOLD = 0.2f;

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		if(!characterController.isGrounded)
		{
			gravityContributionMultiplier += Time.deltaTime * gravityCombackMultiplier;
		}
		if(isJumping)
		{
			if (Time.time >= jumpBeginTime + jumpInputDuration)
			{
				isJumping = false;
				gravityContributionMultiplier = 1f;
			}
			else
			{
				gravityContributionMultiplier *= gravityDivider;
			}
		}
		// 최종 점프 움직임 계산
		if(!characterController.isGrounded)
		{
			movementVector = inputVector * speed;

			gravityContributionMultiplier = Mathf.Clamp01(gravityContributionMultiplier);
			verticalMovement += Physics.gravity.y * gravityMultiplier * Time.deltaTime * gravityContributionMultiplier;

			verticalMovement = Mathf.Clamp(verticalMovement, -maxFallSpeed, 100f);
		}
		else
		{
			// full speed
			movementVector = inputVector * speed;

			if(!isJumping)
			{
				verticalMovement = fallingVerticalMovement;
				gravityContributionMultiplier = 0f;
			}
		}
		UpdateSlide();

		movementVector.y = verticalMovement;
		Debug.Log("1 vertical movement : " + verticalMovement);
		characterController.Move(movementVector * Time.deltaTime);
		// 캐릭터 회전
		movementVector.y = 0f;
		if(movementVector.sqrMagnitude >= ROTATION_THRESHOLD)
		{
			float targetRotation = Mathf.Atan2(movementVector.x, movementVector.z) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
				transform.eulerAngles.y,
				targetRotation,
				ref turnSmoothSpeed,
				turnSmoothTime);
		}
	}

	public void Move(Vector3 movement)
	{
		inputVector = movement;
	}

	public void Jump()
	{
		if(characterController.isGrounded)
		{
			isJumping = true;
			jumpBeginTime = Time.time;
			verticalMovement = initialJumpForce;
			gravityContributionMultiplier = 0f;
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		hitNormal = hit.normal;
		bool isMovingUpwards = verticalMovement > 0f;
		if (isMovingUpwards)
		{
			// Making sure the collision is near the top of the head
			float permittedDistance = characterController.radius / 2f;
			float topPositionY = transform.position.y + characterController.height;
			float distance = Mathf.Abs(hit.point.y - topPositionY);
			if (distance <= permittedDistance)
			{
				// Stopping any upwards movement
				// and having the player fall back down
				isJumping = false;
				gravityContributionMultiplier = 1f;
				verticalMovement = 0f;
			}
		}
	}

	public void CancelJump()
	{
		isJumping = false; //This will stop the reduction to the gravity, which will then quickly pull down the character
	}

	private void UpdateSlide()
	{
		// if player has to slide then add sideways speed to make it go down
		if (shouldSlide)
		{
			movementVector.x += (1f - hitNormal.y) * hitNormal.x * (speed - slideFriction);
			movementVector.z += (1f - hitNormal.y) * hitNormal.z * (speed - slideFriction);
		}
		// check if the controller is grounded and above slope limit
		// if player is grounded and above slope limit
		// player has to slide
		if (characterController.isGrounded)
		{
			currentSlope = Vector3.Angle(Vector3.up, hitNormal);
			shouldSlide = currentSlope >= characterController.slopeLimit;
		}
	}
}
