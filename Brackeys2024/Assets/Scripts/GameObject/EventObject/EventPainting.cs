using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPainting : EventObject
{
    [Tooltip("Must have component: 'RemovableObject'")]
    public GameObject PaintingObject;
    public DialogueSpeaker dialogueSpeaker;

    public string AudioPaintingFall = "int_paintingfall";
    public string AudioPaintingPick = "int_paintingpick";

    private RemovableObject painting;

    private void Start()
    {
        painting = PaintingObject.GetComponent<RemovableObject>();
        painting.HideInteractText = true;

        // 事件发生时，将画设置为自由刚体
        OnEventHappening += () =>
        {
            painting.HideInteractText = false;
            painting.Drop();
            StorySystem.NotReplayable(AudioPaintingFall);
            StorySystem.PlayStoryAudio(AudioPaintingFall);
            dialogueSpeaker.playDialogue();
        };
        // 玩家手持画并且点击触发框时，把画挂回去
        Character.OnPlayerInteract += (player, trans) =>
        {
            if (IsEventOn && player.GrabbingObject == painting && trans == transform)
            {
                player.GrabbingObject = null;
                painting.HideInteractText = true;
                painting.Restore();
                StorySystem.NotReplayable(AudioPaintingPick);
                StorySystem.PlayStoryAudio(AudioPaintingPick);
            }
        };
        // 每次画被重置（被挂回去）时取消事件
        painting.OnRestored += SetEventOff;

        RegisterEventObject();
    }

    public override string GetInteractText() =>
        IsEventOn && Character.Instance.GrabbingObject == painting ? "Place Back" : "";
}
