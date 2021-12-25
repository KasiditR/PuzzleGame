using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ThridPersonController : MonoBehaviour
{

    private Controllers playerActions;
    private InputAction move;

    private Collider cd;
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;
    private float distToGround;
    
    [SerializeField] private Camera playerCamera;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<Collider>();
        playerActions = new Controllers();
        distToGround = cd.bounds.extents.y;
    }
    private void FixedUpdate()
    {

        bool isRun = (playerActions.Player.Shift.ReadValue<float>() > 0 ? true : false);
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * (isRun ? movementForce * 10 : movementForce);
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * (isRun ? movementForce * 10 : movementForce);

        rb.AddForce(forceDirection, ForceMode.Impulse);
        //Animation Movement
        forceDirection = Vector3.zero;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        LookAt();
    }
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }
    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }
    private void OnEnable()
    {
        playerActions.Player.Jump.started += DoJump;
        move = playerActions.Player.Movement;
        playerActions.Player.Enable();
    }
    private void OnDisable()
    {
        playerActions.Player.Jump.started -= DoJump;
        playerActions.Player.Disable();
    }
    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGround())
        {
            forceDirection += Vector3.up * jumpForce;
            //Animation Jump
        }
    }
    private bool IsGround()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
