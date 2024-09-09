using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventObject : MonoBehaviour
{
    public event UnityAction OnEventHappening;
    public event UnityAction OnHandleEvent;

    private bool eventOn = false;
    public bool IsEventOn => eventOn;

    public void SetEventOn()
    {
        eventOn = true;
        OnEventHappening?.Invoke();
    }

    public void SetEventOff()
    {
        eventOn = false;
        OnHandleEvent?.Invoke();
    }
}
