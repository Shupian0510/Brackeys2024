using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class RemovableObject : MonoBehaviour, IInteractive
{
    public event UnityAction OnRestored;

    public RemovableObjectEnum ToolType = RemovableObjectEnum.None;

    private bool restoring = false;
    private bool grabbing = false;
    private Transform grabRoot = null;
    private new Rigidbody rigidbody;
    private new Collider collider;

    private (Vector3 pos, Quaternion quat) transformOriginStatus;
    private (bool, bool, bool) rigidbodyOriginalStatus;

    public bool HideInteractText { get; set; } = false;
    public bool Locked { get; set; } = false;

    public string GetInteractText() => HideInteractText ? "" : "Grab";

    private void Start()
    {
        (rigidbody, collider) = (GetComponent<Rigidbody>(), GetComponent<Collider>());
        transformOriginStatus = (transform.position, transform.rotation);
        rigidbodyOriginalStatus = (
            rigidbody.useGravity,
            rigidbody.isKinematic,
            rigidbody.freezeRotation
        );

        Character.OnPlayerInteract += (player, trans) =>
        {
            // When player interact with 'this' removable
            if (
                trans == transform
                && player.GrabbingObject == null
                && !grabbing
                && !restoring
                && !Locked
            )
            {
                player.GrabbingObject = this;
                // TODO: Hard coded 'Grab Root'
                SetGrab(player.transform.GetChild(0).GetChild(0));
            }
        };
    }

    public void Restore()
    {
        restoring = true;
        grabbing = false;
        collider.enabled = true;
    }

    public void SetGrab(Transform parent)
    {
        if (Locked)
            return;
        grabbing = true;
        collider.enabled = false;
        rigidbody.useGravity = false;
        rigidbody.isKinematic = false;
        rigidbody.freezeRotation = true;
        grabRoot = parent;
    }

    public void Drop()
    {
        grabbing = false;
        collider.enabled = true;
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        rigidbody.freezeRotation = false;
    }

    private void Update()
    {
        if (grabbing)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, grabRoot.position, Time.deltaTime * 10),
                Quaternion.Lerp(transform.rotation, grabRoot.parent.rotation, Time.deltaTime * 10)
            );
            rigidbody.velocity = Vector3.zero;
        }
        else if (restoring)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, transformOriginStatus.pos, Time.deltaTime * 10),
                Quaternion.Lerp(transform.rotation, transformOriginStatus.quat, Time.deltaTime * 10)
            );
            rigidbody.velocity = Vector3.zero;
            if (
                Vector3.Distance(transform.position, transformOriginStatus.pos) < 0.01
                || Quaternion.Angle(transform.rotation, transformOriginStatus.quat) < 0.5f
            )
            {
                restoring = false;
                (transform.position, transform.rotation) = transformOriginStatus;
                (rigidbody.useGravity, rigidbody.isKinematic, rigidbody.freezeRotation) =
                    rigidbodyOriginalStatus;
                OnRestored?.Invoke();
            }
        }
    }
}

public enum RemovableObjectEnum
{
    None,
}
