using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float aimingSpeed = 5f;
    [SerializeField] private Transform cameraTransform;
     
    private CharacterController characterController;
    private Animator animator;
    private float aimingLayerWeight = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        var input = InputManager.instance.move;

        var forward = cameraTransform.forward;
        var right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        var moveDirection = (forward * input.y + right * input.x).normalized;

        var isRunning = InputManager.instance.isRunning;
        var isAiming = InputManager.instance.isAiming;
        var speed = isRunning ? runSpeed : walkSpeed;
        
        animator.SetFloat("Speed", moveDirection.magnitude * (isRunning ? 2.0f : 1.0f));
        animator.SetBool("isRunning", isRunning);

        // Update aiming layer weight
        UpdateAimingWeight(isAiming);

        if (moveDirection.magnitude > 0)
        {
            // Smoothly interpolate the rotation
            var targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        characterController.Move(moveDirection * (speed * Time.deltaTime));
    }

    private void UpdateAimingWeight(bool isAiming)
    {
        var weightChange = Time.deltaTime * aimingSpeed;
        aimingLayerWeight = isAiming ? Mathf.Min(aimingLayerWeight + weightChange, 1f) : Mathf.Max(aimingLayerWeight - weightChange, 0f);
        animator.SetLayerWeight(animator.GetLayerIndex("Aiming"), aimingLayerWeight);
    }
}
