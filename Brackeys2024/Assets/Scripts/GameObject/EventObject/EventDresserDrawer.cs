using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDresserDrawer : MonoBehaviour, IInteractive
{
    public event UnityAction OnDrawerClosed;

    public float OpenDistance;

    private bool opened = false;
    private Vector3 targetPosition;

    public bool Open
    {
        get => opened;
        set
        {
            if (opened != value)
            {
                if (opened)
                    targetPosition.z -= OpenDistance;
                else
                    targetPosition.z += OpenDistance;
                opened = value;
            }
        }
    }

    public string GetInteractText() => opened ? "Close It" : "";

    private void Start()
    {
        targetPosition = transform.position;

        Character.OnPlayerInteract += (_, trans) =>
        {
            if (opened && trans == transform)
            {
                OnDrawerClosed?.Invoke();
                Open = false;
            }
        };
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
    }
}
