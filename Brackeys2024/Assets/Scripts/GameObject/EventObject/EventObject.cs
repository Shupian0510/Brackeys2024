using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventObject : MonoBehaviour, IInteractive
{
    public event UnityAction OnEventHappening;
    public event UnityAction OnHandleEvent;

    private bool eventOn = false;
    public bool IsEventOn => eventOn;

    protected void RegisterEventObject() => EventManager.Instance.RegisterEventObject(this);

    public void SetEventOn()
    {
        if (!eventOn)
        {
            eventOn = true;
            OnEventHappening?.Invoke();
        }
    }

    public void SetEventOff()
    {
        if (eventOn)
        {
            eventOn = false;
            OnHandleEvent?.Invoke();
        }
    }

    public virtual string GetInteractText() => IsEventOn ? "Stop It!" : "";
}
