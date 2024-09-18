using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPainting : EventObject
{
    [Tooltip("Must have component: 'RemovableObject'")]
    public GameObject PaintingObject;
    public DialogueSpeaker dialogueSpeaker;
    private new AudioSource audio;

    public string AudioPaintingFall = "int_paintingfall";
    public string AudioPaintingPick = "int_paintingpick";

    private RemovableObject painting;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        painting = PaintingObject.GetComponent<RemovableObject>();
        painting.HideInteractText = true;
        painting.Locked = true;

        // 事件发生时，将画设置为自由刚体
        OnEventHappening += () =>
        {
            audio.Play();
            painting.HideInteractText = false;
            painting.Locked = false;
            painting.Drop();
            TaskManager.Instance.AddTask(new Task("Pick Painting", "Go find the dropped painting"));
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
                painting.Locked = true;
                painting.Restore();
                StorySystem.NotReplayable(AudioPaintingPick);
                StorySystem.PlayStoryAudio(AudioPaintingPick);
                TaskManager.Instance.RemoveTaskByName("Pick Painting");

                if (StoryFlowControl.state == -2) { 
                    StoryFlowControl.state = 5;
                    StoryFlowControl.Instance.stage0off();
                }
            }
        };
        // 每次画被重置（被挂回去）时取消事件
        painting.OnRestored += SetEventOff;

        RegisterEventObject();
    }

    public override string GetInteractText() =>
        IsEventOn && Character.Instance.GrabbingObject == painting ? "Place Back" : "";
}
