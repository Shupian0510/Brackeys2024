using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Phone : MonoBehaviour, IInteractive
{
    private AudioSource audio;
    private bool isRinging = false;

    // 根据响铃状态自动变更的互动文字
    public string GetInteractText() => isRinging ? "Pick Up" : "";

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        Character.OnPlayerInteract += (_player, trans) =>
        {
            if (trans == transform && isRinging)
            {
                RingingOff();
                OnPickedUp();
            }
        };
    }

    private void OnPickedUp()
    {
        // TODO: 玩家点击电话后的逻辑代码
    }

    // TODO: 音源操作代码需完善
    public void Ringing()
    {
        isRinging = true;
        // audio.Play();
    }

    public void RingingOff()
    {
        isRinging = false;
        // audio.Stop();
    }
}
