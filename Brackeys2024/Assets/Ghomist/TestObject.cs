using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test only
/// Example of using class "EventObject"
/// </summary>
[RequireComponent(typeof(EventObject))]
public class TestObject : MonoBehaviour
{
    private float cd = 3;

    private EventObject @event;

    private void Start()
    {
        @event = GetComponent<EventObject>();
        @event.OnHandleEvent += OnHandleEvent;
    }

    private void OnHandleEvent()
    {
        cd = 5;
        var rot = transform.localEulerAngles;
        rot.z = 0;
        transform.localEulerAngles = rot;
    }

    private void Update()
    {
        if (cd > 0)
        {
            cd -= Time.deltaTime;
            if (cd <= 0)
            {
                @event.SetEventOn();
            }
        }
        else
        {
            var rot = transform.localEulerAngles;
            rot.z = 2 * Mathf.Cos(Time.time * 100);
            transform.localEulerAngles = rot;
        }
    }
}
