using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public enum MoveStateEnum
    {
        Idle,
        Walk,
        Sprint
    }

    public static Character Instance;

    public static event UnityAction<Character, Transform> OnPlayerInteract;
    public static event UnityAction<Character, Transform> OnPlayerLookingAt;
    public static event UnityAction<Character, Transform> OnPlayerHoldingOn;
    public static event UnityAction<Character, Transform> OnPlayerReleaseOn;

    public float MoveSpeed = 3f;
    public float SprintSpeed = 4.5f;
    public float RotateSpeedX = 3f;
    public float RotateSpeedY = 3f;
    public float VerticalAngleLimit = 70f;
    public float InteractDistanceLimit = 3f;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode BreathKey = KeyCode.R;

    public MoveStateEnum MoveState { get; private set; } = MoveStateEnum.Idle;
    public RemovableObject GrabbingObject { get; set; }
    NormalInteractObj currentInteractable;

    private CharacterController controller;
    private Transform playerCam;
    private bool lockCamera = false;
    private bool restoringCamera = false;
    private Vector3 cameraOriginPosition;
    private Quaternion cameraOriginRotation;
    private Transform cameraTarget;

    private void Awake() => Instance = this;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<Camera>().transform;
        cameraOriginPosition = playerCam.position;
        cameraOriginRotation = playerCam.rotation;
    }

    private void Update()
    {
        var breathing = Input.GetKey(BreathKey);

        if (lockCamera)
        {
            if (restoringCamera)
            {
                if (
                    Vector3.Distance(playerCam.position, cameraOriginPosition) < 0.01
                    || Quaternion.Angle(playerCam.rotation, cameraOriginRotation) < 0.5f
                )
                {
                    playerCam.SetPositionAndRotation(cameraOriginPosition, cameraOriginRotation);
                    lockCamera = false;
                    restoringCamera = false;
                }
                playerCam.SetPositionAndRotation(
                    Vector3.Lerp(playerCam.position, cameraOriginPosition, Time.deltaTime * 5),
                    Quaternion.Lerp(playerCam.rotation, cameraOriginRotation, Time.deltaTime * 5)
                );
            }
            else
            {
                playerCam.SetPositionAndRotation(
                    Vector3.Lerp(playerCam.position, cameraTarget.position, Time.deltaTime * 5),
                    Quaternion.Lerp(playerCam.rotation, cameraTarget.rotation, Time.deltaTime * 5)
                );
            }
        }
        else if (!breathing)
        {
            HandleMove();
            HandleCamera();
        }
        if (StressManager.Instance != null)
            StressManager.Instance.Breathing = breathing;
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

        //TODO: Dynamic camera FOV (on sprinting)
    }

    private void HandleInteract()
    {
        var ray = new Ray(playerCam.position, playerCam.forward);
        if (Physics.Raycast(ray, out var hitInfo, InteractDistanceLimit))
        {
            if (hitInfo.collider.tag == "Interactable")
            {
                NormalInteractObj newInteractable =
                    hitInfo.collider.GetComponent<NormalInteractObj>();

                if (currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }
                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
            // Invoke 'look at' event
            OnPlayerLookingAt?.Invoke(this, hitInfo.transform);

            // Invoke 'interact' event
            if (Input.GetMouseButtonDown(0) && currentInteractable != null)
            {
                currentInteractable.Interact();
            }
            if (Input.GetMouseButtonDown(0))
            {
                OnPlayerInteract?.Invoke(this, hitInfo.transform);
            }
            else if (Input.GetMouseButton(0))
            {
                OnPlayerHoldingOn?.Invoke(this, hitInfo.transform);
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnPlayerReleaseOn?.Invoke(this, hitInfo.transform);
            }

            // Update interaction text (of UI)
            if (
                InteractionText.Instance != null
                && hitInfo.transform.TryGetComponent<IInteractive>(out var component)
            )
            {
                InteractionText.Instance.Show = true;
                InteractionText.Instance.Text = component.GetInteractText();
            }
            else
            {
                InteractionText.Instance.Show = false;
            }
        }
        else
        {
            DisableCurrentInteractable();
        }

        // Handle dropping item
        if (GrabbingObject != null && Input.GetKey(KeyCode.Q))
        {
            GrabbingObject.Drop();
            GrabbingObject = null;
        }
    }

    public void SetNewCurrentInteractable(NormalInteractObj newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
    }

    public void DisableCurrentInteractable()
    {
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }

    public void LockCamera(Transform target, bool instantly = false)
    {
        if (!lockCamera)
        {
            cameraOriginPosition = playerCam.position;
            cameraOriginRotation = playerCam.rotation;

            if (instantly)
            {
                playerCam.SetPositionAndRotation(target.position, target.rotation);
            }
        }
        lockCamera = true;
        cameraTarget = target;
    }

    public void RestoreCamera()
    {
        restoringCamera = true;
    }
}
