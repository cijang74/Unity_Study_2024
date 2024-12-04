using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
// 씬 이동할 때 플레이어를 잡아주는 시네머신 카메라를 인스턴스화시켜 삭제되지 않도록 하는 클래스
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start() 
    {
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }
}
