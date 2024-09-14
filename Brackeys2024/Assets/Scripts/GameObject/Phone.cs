using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Phone : MonoBehaviour, IInteractive
{
    private AudioSource audio;
    private bool isRinging = false;

    // 根据响铃状态自动变更的互动文字
    public string GetInteractText() => isRinging ? "call" : "";

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        Character.OnPlayerInteract += (_player, trans) =>
        {
            if (trans == transform && isRinging)
            {
                OnPickedUp();
            }
        };
    }

    private void OnPickedUp()
    {
        ActiveOff();
        if(StoryFlowControl.state == -1) StoryFlowControl.diadoc();
        if (StoryFlowControl.state == 13) StoryFlowControl.diamom();
    }

    // TODO: 音源操作代码需完善
    public void ActiveOn()
    {
        Debug.Log("RINGON");
        isRinging = true;
        audio.Play();
    }

    public void ActiveOff()
    {
        isRinging = false;
        audio.Stop();
    }
}
