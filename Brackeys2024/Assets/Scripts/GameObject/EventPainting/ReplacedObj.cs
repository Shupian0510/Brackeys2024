using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(EventObject))]
public class ReplacedObj : MonoBehaviour
{
    private EventObject @event;
    private Rigidbody rig;



    private void Start()
    {
        @event = GetComponent<EventObject>();
        rig = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (@event.IsEventOn)
        {
            rig.isKinematic = false;
            rig.useGravity = true;
        }
        else {
            rig.useGravity = false;
        }
    }
}
