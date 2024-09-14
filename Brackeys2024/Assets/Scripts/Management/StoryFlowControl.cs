using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StoryFlowControl : MonoBehaviour
{
    public GameObject journal, phone,pill;
    public static int state = 0;
    private static StorySystem Instance;

    public float timer = 0f;
    public float stage1Time = 60f;
    void Start()
    {
        EventManager.Instance.Pause = true;
    }

    void Update()
    {
        if (state == 1 && !StorySystem.HasAudioInQueue())
        {
            //开头语结束之后，日记启动
            journal.GetComponent<Journal>().ActiveOn();
        }
        else if (state == 2 && !StorySystem.HasAudioInQueue())
        {
            //看完日记之后电话响起，等待接通之后对话之后进入第一阶段
            phone.GetComponent<Phone>().ActiveOn();
            state = 0;
        }
        else if (state == 4 && !StorySystem.HasAudioInQueue())
        {
            EventManager.Instance.Pause = false;
            //这里改成画开始落下，标志第一阶段开启
            state = 5;
        }
        else if (state == 5)
        {
            timer += Time.deltaTime;
            if (timer > stage1Time)
            {
                timer = 0f;
                EventManager.Instance.Pause = true;
                state = 6;
            }
        }
        else if (state == 6)
        {
            stage1over();
            pill.SetActive(true);
            state = 0;
        }
        else if (state == 7) {
            //床开启睡觉模式
        }
    }

    public static void startvoice() {
        StorySystem.PlayStoryAudio("beg1");
        StorySystem.PlayStoryAudio("beg2");
        state = 1;
        //test
    }
    public static void diadoc() {
        StorySystem.PlayStoryAudio("Docdia1_doc");
        StorySystem.PlayStoryAudio("Docdia2_player");
        StorySystem.PlayStoryAudio("Docdia3_doc");
        StorySystem.PlayStoryAudio("Docdia4_player");
        StorySystem.PlayStoryAudio("Docdia5_doc");
        state = 4;
    }

    public static void stage1over() {
        StorySystem.PlayStoryAudio("stage1over");
        //StorySystem.PlayStoryAudio("afterpill");
    }

    public static void diamim() {
        StorySystem.PlayStoryAudio("momdia1_mom");
        StorySystem.PlayStoryAudio("momdia2_player");
        StorySystem.PlayStoryAudio("momdia3_mom");
        StorySystem.PlayStoryAudio("momdia4_player");
    }
}
