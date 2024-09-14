using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour, IInteractive
{
    private AudioSource audio;
    private bool active = false;

    // 根据响铃状态自动变更的互动文字
    public string GetInteractText() => active ? "Journal" : "";

    private void Start()
    {
        Character.OnPlayerInteract += (_player, trans) =>
        {
            if (trans == transform && active)
            {
                Oninteract();
            }
        };
    }

    private void Oninteract()
    {
        //Journal Panel On
        StorySystem.PlayStoryAudio("int_journal");
        StoryFlowControl.state = 2;

    }

    // TODO: 音源操作代码需完善
    public void ActiveOn()
    {
        active = true;
        
        // audio.Play();
    }

    public void ActiveOff()
    {
        active = false;
        // audio.Stop();
    }
}
