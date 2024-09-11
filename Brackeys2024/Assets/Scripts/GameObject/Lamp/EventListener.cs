using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventObject))]
public class EventListener: MonoBehaviour
{
    private EventObject @event;

    private void Start()
    {
        @event = GetComponent<EventObject>();
    }

    private void Update()
    {
        //Swing Control
        //var rot = transform.localEulerAngles;
        //rot.z = @event.IsEventOn ? 2 * Mathf.Cos(Time.time * 100) : 0;
        //transform.localEulerAngles = rot;
    }
}
