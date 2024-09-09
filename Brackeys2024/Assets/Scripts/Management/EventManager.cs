using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public bool Pause;
    public float EventIntervalMin;
    public float EventIntervalMax;
    public float EventPossibility;

    public int EventCount => eventObjects.Count(x => x.IsEventOn);

    private float eventTimer = 0;
    private readonly List<EventObject> eventObjects = new();

    private void Awake() => Instance = this;

    public void RegisterEventObject(EventObject e)
    {
        eventObjects.Add(e);
    }

    private void Start()
    {
        eventTimer += RandomInterval;
    }

    private void Update()
    {
        if (Pause)
            return;

        if (eventTimer > 0)
        {
            eventTimer -= Time.deltaTime;
        }
        else
        {
            eventTimer += RandomInterval;
            if (CanHappened)
            {
                eventObjects[Random.Range(0, eventObjects.Count)].SetEventOn();
                print("Happened");
            }
            else
            {
                print("Not happened");
            }
        }
    }

    private float RandomInterval => Random.Range(EventIntervalMin, EventIntervalMax);
    private bool CanHappened => Random.value < EventPossibility;
}
