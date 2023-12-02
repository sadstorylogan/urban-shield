using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Input;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook regularCamera;
    [SerializeField] private CinemachineVirtualCamera aimCamera;

    private void Update()
    {
        if (InputManager.instance.isAiming)
        {
            aimCamera.gameObject.SetActive(true);
            regularCamera.gameObject.SetActive(false);
        }
        else
        {
            aimCamera.gameObject.SetActive(false);
            regularCamera.gameObject.SetActive(true);
        }
    }
}
