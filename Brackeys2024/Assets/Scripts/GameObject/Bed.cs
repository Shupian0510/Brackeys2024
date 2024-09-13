using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractive
{
    public GameObject SleepCameraRoot;

    public string GetInteractText() => "Sleep";

    private bool sleeping = false;
    private Character player;

    private void Start()
    {
        Character.OnPlayerInteract += (player, trans) =>
        {
            this.player = player;
            player.LockCamera(SleepCameraRoot.transform);
            Invoke(nameof(ToggleState), 2f);
        };
    }

    private void Update()
    {
        if (sleeping && Input.anyKeyDown)
        {
            player.RestoreCamera();
            ToggleState();
        }
    }

    private void ToggleState() => sleeping = !sleeping;
}
