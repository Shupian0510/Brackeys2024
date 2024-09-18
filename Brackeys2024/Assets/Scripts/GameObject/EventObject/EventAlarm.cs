using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAlarm : MonoBehaviour, IInteractive
{
    public event UnityAction OnAlarmOff;
    public AudioSource audio;

    private bool ringing = false;
    public bool Ringing
    {
        get => ringing;
        set { 
            ringing = value; 
            audio.Play();
        }
    }

    public string GetInteractText() => ringing ? "Stop..." : "";

    private void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        Character.OnPlayerInteract += (_, trans) =>
        {
            if (ringing && trans == transform)
            {
                OnAlarmOff?.Invoke();
                Ringing = false;
                audio.Stop();
                TaskManager.Instance.RemoveTaskByName("Find Alarm");

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
