using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAlarmParent : EventObject
{
    public List<GameObject> AlarmObjectList = new();

    private List<EventAlarm> alarms;

    private void Start()
    {
        AlarmObjectList.ForEach(x => x.AddComponent<EventAlarm>());

        alarms = AlarmObjectList.ConvertAll(x => x.GetComponent<EventAlarm>());
        alarms.ForEach(x => x.OnAlarmOff += SetEventOff);

        OnEventHappening += () =>
        {
            if (alarms.Count > 0)
            {
                var target = alarms[Random.Range(0, alarms.Count)];
                target.Ringing = true;
            }
        };

        RegisterEventObject();
    }
}
