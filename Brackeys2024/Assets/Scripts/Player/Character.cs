using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public float SprintSpeed = 4.5f;
    public float RotateSpeedX = 3f;
    public float RotateSpeedY = 3f;
    public float VerticalAngleLimit = 70f;
    public float InteractDistanceLimit = 3f;
    public KeyCode SprintKey = KeyCode.LeftShift;

    private RemovableObject grabbingObject = null;
    private CharacterController controller;
    private Camera playerCam;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        var x = Input.GetAxis("Horizontal") * transform.right;
        var z = Input.GetAxis("Vertical") * transform.forward;
        var sprint = Input.GetKey(SprintKey);
        var move = (x + z) * (sprint ? SprintSpeed : MoveSpeed);
        controller.SimpleMove(move);
        if (sprint)
        {
            print("Player is sprinting!");
        }

        var mx = Input.GetAxis("Mouse X");
        transform.Rotate(mx * RotateSpeedX * Vector3.up, Space.World);

        var camTrans = playerCam.transform;
        var my = Input.GetAxis("Mouse Y");
        camTrans.Rotate(-my * RotateSpeedY * Vector3.right);
        // Clamp rotation
        var angle = camTrans.localEulerAngles;
        camTrans.localEulerAngles = new(
            angle.x > 180
                ? Mathf.Clamp(angle.x, 360 - VerticalAngleLimit, 360)
                : Mathf.Clamp(angle.x, 0, VerticalAngleLimit),
            0,
            0
        );

        var ray = new Ray(camTrans.position, camTrans.forward);
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

        if (grabbingObject != null && Input.GetKey(KeyCode.Q))
        {
            grabbingObject.Drop();
            grabbingObject = null;
        }

        // Adjust player's FOV, but may conflict with "CameraZoom"
        // if (Input.GetKeyDown(SprintKey))
        // {
        //     playerCam.fieldOfView *= 1.2f;
        // }
        // else if (Input.GetKeyUp(SprintKey))
        // {
        //     playerCam.fieldOfView /= 1.2f;
        // }
    }
}
