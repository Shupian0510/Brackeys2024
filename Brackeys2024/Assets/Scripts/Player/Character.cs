using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public float MoveSpeed = 4f;
    public float RotateSpeedX = 3f;
    public float RotateSpeedY = 3f;
    public float VerticalAngleLimit = 60f;

    public float InteractDistanceLimit = 3f;

    private CharacterController controller;
    private Camera playerCam;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        var x = Input.GetAxis("Horizontal") * MoveSpeed * transform.right;
        var z = Input.GetAxis("Vertical") * MoveSpeed * transform.forward;
        controller.SimpleMove(x + z);

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
        if (
            Physics.Raycast(ray, out var hitInfo, InteractDistanceLimit)
            && hitInfo.transform.TryGetComponent<EventObject>(out var e)
            && e.IsEventOn
        )
        {
            InteractionText.Instance.Show = true;
            if (Input.GetMouseButtonDown(0))
            {
                e.SetEventOff();
            }
        }
        else
        {
            InteractionText.Instance.Show = false;
        }
    }
}
