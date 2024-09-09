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
    private EventObject @event;

    private void Start()
    {
        @event = GetComponent<EventObject>();
    }

    private void Update()
    {
        var rot = transform.localEulerAngles;
        rot.z = @event.IsEventOn ? 2 * Mathf.Cos(Time.time * 100) : 0;
        transform.localEulerAngles = rot;
    }
}
