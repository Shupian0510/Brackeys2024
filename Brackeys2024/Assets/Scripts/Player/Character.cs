using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public enum MoveStateEnum
    {
        Idle,
        Walk,
        Sprint
    }

    public float MoveSpeed = 3f;
    public float SprintSpeed = 4.5f;
    public float RotateSpeedX = 3f;
    public float RotateSpeedY = 3f;
    public float VerticalAngleLimit = 70f;
    public float InteractDistanceLimit = 3f;
    public KeyCode SprintKey = KeyCode.LeftShift;

    public MoveStateEnum MoveState { get; private set; } = MoveStateEnum.Idle;

    private RemovableObject grabbingObject = null;
    private CharacterController controller;
    private Transform playerCam;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        HandleMove();
        HandleCamera();
        HandleInteract();
    }

    private void HandleMove()
    {
        var x = Input.GetAxis("Horizontal") * transform.right;
        var z = Input.GetAxis("Vertical") * transform.forward;
        var sprint = Input.GetKey(SprintKey);
        var move = (x + z) * (sprint ? SprintSpeed : MoveSpeed);
        controller.SimpleMove(move);

        MoveState =
            move.sqrMagnitude > 0.01
                ? MoveStateEnum.Idle
                : sprint
                    ? MoveStateEnum.Sprint
                    : MoveStateEnum.Walk;
    }

    private void HandleCamera()
    {
        var mx = Input.GetAxis("Mouse X");
        transform.Rotate(mx * RotateSpeedX * Vector3.up, Space.World);

        var my = Input.GetAxis("Mouse Y");
        playerCam.Rotate(-my * RotateSpeedY * Vector3.right);
        // Clamp rotation
        var angle = playerCam.localEulerAngles;
        playerCam.localEulerAngles = new(
            angle.x > 180
                ? Mathf.Clamp(angle.x, 360 - VerticalAngleLimit, 360)
                : Mathf.Clamp(angle.x, 0, VerticalAngleLimit),
            0,
            0
        );

        //TODO: Handle camera FOV (dynamic FOV on sprinting)
    }

    private void HandleInteract()
    {
        var ray = new Ray(playerCam.position, playerCam.forward);
        if (Physics.Raycast(ray, out var hitInfo, InteractDistanceLimit))
        {
            if (hitInfo.transform.TryGetComponent<EventObject>(out var e) && e.IsEventOn)
            {
                InteractionText.Instance.Show = true;
                InteractionText.Instance.Text = "Stop It !";
                if (Input.GetMouseButtonDown(0))
                {
                    e.SetEventOff();
                }
            }
            else if (
                grabbingObject == null
                && hitInfo.transform.TryGetComponent<RemovableObject>(out var o)
            )
            {
                InteractionText.Instance.Show = true;
                InteractionText.Instance.Text = "Grab";
                if (Input.GetMouseButton(0))
                {
                    grabbingObject = o;
                    grabbingObject.SetGrab(transform.GetChild(0).GetChild(0));
                }
            }
            else
            {
                InteractionText.Instance.Show = false;
            }
        }
        else
        {
            InteractionText.Instance.Show = false;
        }

        // Handle grabbing
        if (grabbingObject != null && Input.GetKey(KeyCode.Q))
        {
            grabbingObject.Drop();
            grabbingObject = null;
        }
    }
}
