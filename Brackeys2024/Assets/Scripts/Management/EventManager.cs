using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public bool _pause = false;
    public bool Pause
    {
        get => _pause;
        set
        {
            if (StressManager.Instance != null)
                StressManager.Instance.Pause = value;
            _pause = value;
        }
    }
    public float EventIntervalMin;
    public float EventIntervalMax;
    public float EventPossibility;
    public float StressGrowingThreshold = 5f; // 不超过该时间的事件不会造成压力，超过后按时长对数增长

    public int EventCount => eventObjects.Count(x => x.IsMakingStress);
    public float EventHappenRate => (float)EventCount / eventObjects.Count;
    public float StressCount =>
        eventObjects
            .FindAll(x => x.IsMakingStress)
            .Sum(x => Mathf.Log(Mathf.Max(1, x.RemainingTime - StressGrowingThreshold + 1), 2f));

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
                var list = eventObjects
                    .Where(x => x.gameObject.activeInHierarchy && !x.IsEventOn)
                    .ToList();

                list[Random.Range(0, list.Count)].SetEventOn();
            }
            else { }
        }
    }

    private float RandomInterval => Random.Range(EventIntervalMin, EventIntervalMax);
    private bool CanHappened => Random.value < EventPossibility;
}
