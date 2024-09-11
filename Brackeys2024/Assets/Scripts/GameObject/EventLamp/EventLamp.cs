using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 直接继承 EventObject (可以直接访问成员函数)
// 挂组件时无需再挂 EventObject，只挂 EventLamp 即可
public class EventLamp : EventObject
{
    public GameObject Spotlight;
    public float BlinkInterval;

    private float timer = 0;
    private new Renderer renderer;
    private new AudioSource audio;
    private bool isPlaying = false;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        audio = GetComponent<AudioSource>();

        // 监听角色事件：当玩家与 this 互动时（鼠标左键点击），取消事件
        Character.OnPlayerInteract += (_, trans) =>
        {
            if (trans == transform)
            {
                // 这里相当于是让「事件取消的时机」由「事件物体本身」进行控制
                // 也可以不监听此事件，在 Update 中合适的时机进行取消
                SetEventOff();
            }
        };

        // 注册 this 事件对象，使其能被 EventManager 管理
        // 也可以不注册，测试时手动在 Update 中调用 SetEventOn，但不注册无法关联至压力值
        RegisterEventObject();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= BlinkInterval)
        {
            RandomBlink();
            timer = 0f;
        }
    }

    private void RandomBlink()
    {
        if (IsEventOn)
        {
            if (isPlaying)
            {
                audio.Play();
                isPlaying = false;
            }
            bool vis = Random.Range(0, 2) == 0;
            if (vis)
            {
                Spotlight.SetActive(false);
                renderer.material.DisableKeyword("_EMISSION");
            }
            else
            {
                Spotlight.SetActive(true);
                renderer.material.EnableKeyword("_EMISSION");
            }
        }
        else
        {
            if (!isPlaying)
            {
                audio.Stop();
                isPlaying = true;
            }
            Spotlight.SetActive(true);
            renderer.material.EnableKeyword("_EMISSION");
        }
    }
}
