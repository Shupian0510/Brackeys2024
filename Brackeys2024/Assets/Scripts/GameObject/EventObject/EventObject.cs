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

    private float startTime = -1;
    public float RemainingTime => Time.time - startTime;

    public bool IsMakingStress =>
        IsEventOn && RemainingTime > EventManager.Instance.StressGrowingThreshold;

    protected void RegisterEventObject() => EventManager.Instance.RegisterEventObject(this);

    public void SetEventOn()
    {
        if (!eventOn)
        {
            eventOn = true;
            startTime = Time.time;
            OnEventHappening?.Invoke();
        }
    }

    public void SetEventOff()
    {
        if (eventOn)
        {
            eventOn = false;
            startTime = -1;
            OnHandleEvent?.Invoke();
        }
    }

    public virtual string GetInteractText() => IsEventOn ? "Stop It!" : "";
}
