using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToilet : EventObject
{
    private new AudioSource audio;
    private bool isplaying = false;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        Character.OnPlayerInteract += (_, trans) =>
        {
            if (IsEventOn && trans == transform)
            {
                SetEventOff();
                audio.Stop();
                isplaying = false;
            }
        };

        RegisterEventObject();
    }

    private void Update()
    {
        if (! isplaying && IsEventOn) { 
            audio.Play();
            isplaying = true;
        }
        var rot = transform.localEulerAngles;
        rot.z = IsEventOn ? 2 * Mathf.Cos(Time.time * 100) : 0;
        transform.localEulerAngles = rot;
        // TODO: 马桶盖震动的声音
    }
}
