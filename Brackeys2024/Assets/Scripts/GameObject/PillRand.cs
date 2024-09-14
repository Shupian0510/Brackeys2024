using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillRand : MonoBehaviour, IInteractive
{
    private AudioSource audio;
    private bool active = true;

    // 根据响铃状态自动变更的互动文字
    public string GetInteractText() => active ? "Pill" : "";

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
        //Calm Down
        StressManager.Instance.CalmDown(StressManager.Instance.Stress * 0.5f);

        gameObject.SetActive(false);
        MedManager.Instance.UseMedicine();

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
