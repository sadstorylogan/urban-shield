using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

// ReSharper disable All

public class ShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform vfxHitEnemy;
    [SerializeField] private Transform vfxHitElse;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Transform muzzleTransform;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;

    private const float MaxRayDistance = 999f;
    private const float AimLerpSpeed = 10f;
    private const float RotateLerpSpeed = 20f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        HandleAiming();
        HandleShooting();
    }
    
    private void HandleAiming()
    {
        var mouseWorldPosition = GetMouseWorldPosition();

        if (starterAssetsInputs.aim)
        {
            EnterAimMode(mouseWorldPosition);
        }
        else
        {
            ExitAimMode();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        var screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        var ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, MaxRayDistance, aimColliderLayerMask))
        {
            //debugTransform.position = raycastHit.point;
            return raycastHit.point;
        }

        return Vector3.zero;
    }

    private void EnterAimMode(Vector3 mouseWorldPosition)
    {
        aimVirtualCamera.gameObject.SetActive(true);
        thirdPersonController.SetSensitivity(aimSensitivity);
        thirdPersonController.SetRotateOnMove(false);
        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * AimLerpSpeed));

        AlignCharacterWithAim(mouseWorldPosition);
    }

    private void ExitAimMode()
    {
        aimVirtualCamera.gameObject.SetActive(false);
        thirdPersonController.SetSensitivity(normalSensitivity);
        thirdPersonController.SetRotateOnMove(true);
        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * AimLerpSpeed));
    }

    private void AlignCharacterWithAim(Vector3 targetPosition)
    {
        targetPosition.y = transform.position.y;
        var aimDirection = (targetPosition - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * RotateLerpSpeed);
    }
    
    private void HandleShooting()
    {
        if (starterAssetsInputs.aim && starterAssetsInputs.shoot)
        {
            PerformShootAction();
            starterAssetsInputs.shoot = false;
        }
    }

    private void PerformShootAction()
    {
        var hitTransform = GetHitTransform();
        var hitPoint = GetMouseWorldPosition();
        var projectileInstance = Instantiate(projectilePrefab, muzzleTransform.position, muzzleTransform.rotation);

        
        if (hitPoint != Vector3.zero)
        {
            var direction = (hitPoint - transform.position).normalized;
            projectileInstance.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed, ForceMode.VelocityChange);
        }
        
        if (hitTransform != null)
        {
            if (hitTransform.GetComponent<BulletTargetIdentify>() != null)
            {
                Instantiate(vfxHitEnemy, hitPoint, Quaternion.identity);
            }
            else
            {
                Instantiate(vfxHitElse, hitPoint, Quaternion.identity);
            }
        }
    }

    private Transform GetHitTransform()
    {
        var mouseWorldPosition = GetMouseWorldPosition();
        var ray = Camera.main.ScreenPointToRay(mouseWorldPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, MaxRayDistance, aimColliderLayerMask))
        {
            return hit.transform;
        }

        return null;
    }
}