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
    
    private CharacterController characterController;
    private Animator animator;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var input = InputManager.instance.move;
        var moveDirection = new Vector3(input.x, 0, input.y);

        var isRunning = InputManager.instance.isRunning;
        var speed = isRunning ? runSpeed : walkSpeed;
        
        animator.SetFloat("Speed", moveDirection.magnitude * (isRunning ? 2.0f : 1.0f));
        animator.SetBool("isRunning", isRunning);

        if (moveDirection.magnitude > 0)
        {
            // Smoothly interpolate the rotation
            var targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        characterController.Move(moveDirection * (speed * Time.deltaTime));
    }
    
}
