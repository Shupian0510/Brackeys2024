using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAlarm : MonoBehaviour, IInteractive
{
    public event UnityAction OnAlarmOff;

    private bool ringing = false;
    public bool Ringing
    {
        get => ringing;
        set { ringing = value; }
    }

    public string GetInteractText() => ringing ? "Stop..." : "";

    private void Start()
    {
        Character.OnPlayerInteract += (_, trans) =>
        {
            if (ringing && trans == transform)
            {
                OnAlarmOff?.Invoke();
                Ringing = false;
                // TODO: 在这里播放闹钟声音
            }
        };
    }

    private void Update()
    {
        var rot = transform.localEulerAngles;
        rot.z = ringing ? 2 * Mathf.Cos(Time.time * 100) : 0;
        transform.localEulerAngles = rot;
    }
}
