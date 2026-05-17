using Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Start()
    {
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        if (cinemachineVirtualCamera == null)
        {
            cinemachineVirtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        }

        if (cinemachineVirtualCamera == null || PlayerController.Instance == null)
        {
            return;
        }

        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }
}
