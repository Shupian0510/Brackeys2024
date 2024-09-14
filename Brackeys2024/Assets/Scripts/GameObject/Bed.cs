using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractive
{
    public GameObject SleepCameraRoot;
    public GameObject SleepCameraRootTemp;

    public string GetInteractText() => sleeping ? "Get Up" : "Sleep";

    private bool sleeping = false;
    private Character player;

    private void Start()
    {

        Character.OnPlayerInteract += (player, trans) =>
        {
            if (trans == transform && !sleeping)
            {
                this.player = player;
                LockOnTemp();
                Invoke(nameof(LockOnRoot), 1f);
                Invoke(nameof(ToggleState), 2f);

                Debug.Log("SLEEP");
                
                if (StoryFlowControl.state == 7) { 
                    StoryFlowControl.state = 8;
                }
                if (StoryFlowControl.state == 13)
                {
                    StoryFlowControl.state = 14;
                }
            }
        };
    }

    private void Update()
    {
        if (sleeping && Input.anyKeyDown)
        {
            LockOnTemp();
            Invoke(nameof(Unlock), 1f);
            Invoke(nameof(ToggleState), 1f);
        }
    }

    private void LockOnRoot() => player.LockCamera(SleepCameraRoot.transform);

    private void LockOnTemp() => player.LockCamera(SleepCameraRootTemp.transform);

    private void Unlock() => player.RestoreCamera();

    private void ToggleState() => sleeping = !sleeping;
}
