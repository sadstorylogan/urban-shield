using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var input = InputManager.instance.move;
        var moveDirection = new Vector3(input.x, 0, input.y);
        characterController.Move(moveDirection * (moveSpeed * Time.deltaTime));
    }
}
