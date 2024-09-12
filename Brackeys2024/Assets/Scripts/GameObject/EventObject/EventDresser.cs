using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventDresser : EventObject
{
    public float DrawerOpenDistance = 0.2f;
    public List<GameObject> DrawerObjectList = new();

    private List<EventDresserDrawer> drawers;

    public override string GetInteractText() => "";

    private void Start()
    {
        // Apply component on children drawers
        DrawerObjectList.ForEach(x => x.AddComponent<EventDresserDrawer>());

        // Save component list
        drawers = DrawerObjectList.ConvertAll(x => x.GetComponent<EventDresserDrawer>());
        drawers.ForEach(x =>
        {
            x.OpenDistance = DrawerOpenDistance;
            // listen on drawer's 'closing event'
            x.OnDrawerClosed += SetEventOff;
        });

        // Event can only trigger once, so only one drawer will be opened
        OnEventHappening += () =>
        {
            if (drawers.Count > 0)
            {
                var target = drawers[Random.Range(0, drawers.Count)];
                target.Open = true;
            }
        };

        // Use global event manager
        RegisterEventObject();
    }
}
