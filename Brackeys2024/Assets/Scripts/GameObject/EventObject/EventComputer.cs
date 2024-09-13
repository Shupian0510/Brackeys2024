using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class EventComputer : EventObject
{
    private new MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();

        OnEventHappening += () =>
        {
            renderer.material.EnableKeyword("_EMISSION");
        };
        Character.OnPlayerInteract += (_, trans) =>
        {
            if (trans == transform)
            {
                renderer.material.DisableKeyword("_EMISSION");
                SetEventOff();
            }
        };

        RegisterEventObject();
    }
}
