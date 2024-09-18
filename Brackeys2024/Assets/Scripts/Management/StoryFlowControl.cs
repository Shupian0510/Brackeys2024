using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryFlowControl : MonoBehaviour
{
    public GameObject journal, phone,pill;
    public static int state = 7;
    public static StoryFlowControl Instance;
    public GameObject congra;
    public GameObject evon, evoff;

    public float timer = 0f;
    public float stage1Time = 20f;
    public float stage2Time = 20f;
    private void Awake() => Instance = this;
    void Start()
    {
        EventManager.Instance.Pause = true;
        TaskManager.Instance.AddTask(new Task("Get up", "get up [SPACE] and look for the journal"));
    }

    void Update()
    {
        if (state == 1 && !StorySystem.HasAudioInQueue())
        {
            //开头语结束之后，日记启动,可以起床
            Bed.Instance.GetUpLock = false;
            
            journal.GetComponent<Journal>().ActiveOn();
            
        }
        else if (state == 2 && !StorySystem.HasAudioInQueue())
        {
            //看完日记之后电话响起，等待接通之后对话之后进入第一阶段
            //phone.GetComponent<Phone>().ActiveOn();
            TaskManager.Instance.RemoveTaskByName("Get up");
            TaskManager.Instance.AddTask(new Task("Pick up Phone", "Go pick up phone on desk"));
            state = -1;
        }
        else if (state == 4 && !StorySystem.HasAudioInQueue())
        {
            TaskManager.Instance.RemoveTaskByName("Pick up Phone");
            TaskManager.Instance.AddTask(new Task("!!!KEEP CALM!!!", ""));
            EventManager.Instance.Pause = false;
            HR.Instance.PlayHR();
            //这里改成画开始落下，标志第一阶段开启
            state = -2;
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
            TaskManager.Instance.AddTask(new Task("Find Pill in Bathroom", ""));
            TaskManager.Instance.AddTask(new Task("Go to Sleep", ""));
            state = -4;
        }
        else if (state == 8)
        {
            HR.Instance.StopHR();
            SceneSwitcher.Instance.SwitchScene();
            //床开启睡觉模式
            //专场到第二阶段
            state = 9;
        }
        else if (state == 9)
        {
            TaskManager.Instance.RemoveTaskByName("Go to Sleep");
            TaskManager.Instance.RemoveTaskByName("Find Pill in Bathroom");
            EventManager.Instance.Pause = false;
            HR.Instance.PlayHR();
            state = 10;
            MedManager.Instance.RespawnMedicine();
            TaskManager.Instance.AddTask(new Task("Find Drug", "Find drugs and take them to make you calm"));
            //第二阶段开启
        }
        else if (state == 10)
        {
            timer += Time.deltaTime;
            if (timer > stage2Time)
            {
                timer = 0f;
                EventManager.Instance.Pause = true;
                state = 11;
            }
        }
        else if (state == 11)
        {
            HR.Instance.StopHR();
            state = 12;
            TaskManager.Instance.RemoveTaskByName("Find Drug");
        }
        else if (state == 12)
        {
            TaskManager.Instance.AddTask(new Task("Pick up phone", ""));
            TaskManager.Instance.AddTask(new Task("Go to Sleep", ""));
            phone.GetComponent<Phone>().ActiveOn();
            state = 13;
        }
        else if (state == 14)
        {
            TaskManager.Instance.RemoveTaskByName("Pick up phone");
            TaskManager.Instance.RemoveTaskByName("Go to Sleep");
            SceneSwitcher.Instance.SwitchScene();
            congra.SetActive(true);
            state = 15;
        }
        else if (state == 15)
        {
            timer += Time.deltaTime;
            if (timer > 10f)
            {
                timer = 0f;
                StressManager.Instance.IncreasingRate *= 2;
                LevelLoader.Instance.LoadPreviousLevel();
            }
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

    public static void diamom() {
        StorySystem.PlayStoryAudio("momdia1_mom");
        StorySystem.PlayStoryAudio("momdia2_player");
        StorySystem.PlayStoryAudio("momdia3_mom");
        StorySystem.PlayStoryAudio("momdia4_player");
    }
    public void stage0off() {
        evoff.SetActive(false);
        evon.SetActive(true);
    }
}
