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

    private bool waitingHolding = false;
    private float holdingTime = 0;
    private float heldT = 0;
    public float FixingProgress => holdingTime == 0 ? 0 : heldT / holdingTime;

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
            OnHandleEvent?.Invoke();
            if (waitingHolding)
            {
                Character.OnPlayerHoldingOn -= HoldingOn;
                Character.OnPlayerReleaseOn -= OffHolding;
                holdingTime = 0;
                waitingHolding = false;
                if (ProgressSlider.Instance != null)
                    ProgressSlider.Instance.Show = false;
            }
        }
    }

    public void SetEventOff(float holdingTime)
    {
        if (eventOn && !waitingHolding)
        {
            waitingHolding = true;
            this.holdingTime = holdingTime;
            heldT = 0;
            Character.OnPlayerHoldingOn += HoldingOn;
            Character.OnPlayerReleaseOn += OffHolding;
        }
    }

    public virtual string GetInteractText() => IsEventOn ? "Stop It!" : "";

    private void HoldingOn(Character _, Transform trans)
    {
        if (trans == transform)
        {
            heldT += Time.deltaTime;
            if (ProgressSlider.Instance != null)
            {
                ProgressSlider.Instance.Show = true;
                ProgressSlider.Instance.Value = FixingProgress;
            }
            if (heldT >= holdingTime)
            {
                SetEventOff();
            }
        }
        else
        {
            OffHolding(_, trans);
        }
    }

    private void OffHolding(Character _, Transform trans)
    {
        heldT = 0;
        if (ProgressSlider.Instance != null)
        {
            ProgressSlider.Instance.Show = false;
        }
    }
}
