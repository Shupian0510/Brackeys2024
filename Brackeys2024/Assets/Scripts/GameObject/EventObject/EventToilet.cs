using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToilet : EventObject
{
    private void Start()
    {
        Character.OnPlayerInteract += (_, trans) =>
        {
            if (IsEventOn && trans == transform)
            {
                SetEventOff();
            }
        };

        RegisterEventObject();
    }

    private void Update()
    {
        var rot = transform.localEulerAngles;
        rot.z = IsEventOn ? 2 * Mathf.Cos(Time.time * 100) : 0;
        transform.localEulerAngles = rot;
        // TODO: 马桶盖震动的声音
    }
}
