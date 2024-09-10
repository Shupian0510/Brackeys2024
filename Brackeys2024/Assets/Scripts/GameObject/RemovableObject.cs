using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class RemovableObject : MonoBehaviour
{
    public RemovableObjectEnum ToolType = RemovableObjectEnum.None;

    private bool grabbing = false;
    private Transform parent = null;
    private new Rigidbody rigidbody;
    private new Collider collider;

    private void Start() =>
        (rigidbody, collider) = (GetComponent<Rigidbody>(), GetComponent<Collider>());

    public void SetGrab(Transform parent)
    {
        grabbing = true;
        collider.enabled = false;
        rigidbody.useGravity = false;
        rigidbody.freezeRotation = true;
        this.parent = parent;
    }

    public void Drop()
    {
        grabbing = false;
        collider.enabled = true;
        rigidbody.useGravity = true;
        rigidbody.freezeRotation = false;
    }

    private void Update()
    {
        if (grabbing)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                parent.position,
                Time.deltaTime * 10
            );
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                parent.parent.rotation,
                Time.deltaTime * 10
            );
            rigidbody.velocity = Vector3.zero;
        }
    }
}

public enum RemovableObjectEnum
{
    None,
}
