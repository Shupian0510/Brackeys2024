using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlant : EventObject
{
    [Tooltip("Must have component: 'RemovableObject'")]
    public GameObject PaintingObject;

    private RemovableObject painting;

    private void Start()
    {
        painting = PaintingObject.GetComponent<RemovableObject>();
        painting.HideInteractText = true;

        OnEventHappening += () =>
        {
            painting.HideInteractText = false;
            painting.Drop();
        };
        Character.OnPlayerInteract += (player, trans) =>
        {
            if (IsEventOn && player.GrabbingObject == painting && trans == transform)
            {
                player.GrabbingObject = null;
                painting.HideInteractText = true;
                painting.Restore();
            }
        };
        painting.OnRestored += SetEventOff;

        RegisterEventObject();
    }

    public override string GetInteractText() =>
        IsEventOn && Character.Instance.GrabbingObject == painting ? "Place Back" : "";
}
