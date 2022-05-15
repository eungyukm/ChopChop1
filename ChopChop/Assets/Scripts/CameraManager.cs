using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
	public InputReader inputReader;
	public Camera mainCamera;
	public CinemachineFreeLook freeLookVCam;
	public float xAxisSpeed = 3000f;
	public float yAxisSpeed = 10f;

	public void SetupProtagonistVirtualCamera(Transform target)
	{
		freeLookVCam.Follow = target;
		freeLookVCam.LookAt = target;
	}

	private void OnEnable()
	{
		inputReader.cameraMoveEvent += OnCameraMove;
	}

	private void OnDisable()
	{
		inputReader.cameraMoveEvent -= OnCameraMove;
	}

	private void OnCameraMove(Vector2 cameraMovement)
	{
		freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * xAxisSpeed * Time.smoothDeltaTime;
		freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * yAxisSpeed * Time.smoothDeltaTime;
	}
}
