using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class EventComputer : EventObject
{
    private new MeshRenderer renderer;
    private new AudioSource audio;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        audio = GetComponent<AudioSource>();

        OnEventHappening += () =>
        {
            renderer.material.EnableKeyword("_EMISSION");
            audio.Play();
        };
        Character.OnPlayerInteract += (_, trans) =>
        {
            if (trans == transform)
            {
                SetEventOff(2);
            }
        };
        OnHandleEvent += () => renderer.material.DisableKeyword("_EMISSION");

        RegisterEventObject();
    }
}
