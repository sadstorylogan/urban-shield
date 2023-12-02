using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Input;
using UnityEngine;

public class CameraAimZoom : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook regularCam;
    [SerializeField] private CinemachineVirtualCamera aimingCam;

    private void Update()
    {
        if (InputManager.instance.isAiming)
        {
            aimingCam.Priority = 11;
            regularCam.Priority = 10;
        }
        else
        {
            aimingCam.Priority = 9;
            regularCam.Priority = 10;
        }
    }
}
