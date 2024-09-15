using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryFlowControl : MonoBehaviour
{
    public GameObject journal, phone,pill;
    public static int state = 7;
    private static StorySystem Instance;
    public GameObject congra;

    public float timer = 0f;
    public float stage1Time = 20f;
    public float stage2Time = 20f;
    void Start()
    {
        EventManager.Instance.Pause = true;
    }

    void Update()
    {
        if (state == 1 && !StorySystem.HasAudioInQueue())
        {
            //��ͷ�����֮���ռ�����,������
            Bed.Instance.GetUpLock = false;
            journal.GetComponent<Journal>().ActiveOn();
        }
        else if (state == 2 && !StorySystem.HasAudioInQueue())
        {
            //�����ռ�֮��绰���𣬵ȴ���֮ͨ��Ի�֮������һ�׶�
            phone.GetComponent<Phone>().ActiveOn();
            state = -1;
        }
        else if (state == 4 && !StorySystem.HasAudioInQueue())
        {
            EventManager.Instance.Pause = false;
            HR.Instance.PlayHR();
            //����ĳɻ���ʼ���£���־��һ�׶ο���
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
            state = 7;
        }
        else if (state == 8)
        {
            HR.Instance.StopHR();
            SceneSwitcher.Instance.SwitchScene();
            //������˯��ģʽ
            //ר�����ڶ��׶�
            state = 9;
        }
        else if (state == 9)
        {
            EventManager.Instance.Pause = false;
            HR.Instance.PlayHR();
            state = 10;
            MedManager.Instance.RespawnMedicine();
            //�ڶ��׶ο���
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
        }
        else if (state == 12)
        {
            phone.GetComponent<Phone>().ActiveOn();
            state = 13;
        }
        else if (state == 14)
        {
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
                LevelLoader.Instance.LoadPreviousLevel();
            }
        }
    }

    public static void startvoice() {
        //StorySystem.PlayStoryAudio("beg1");
        //StorySystem.PlayStoryAudio("beg2");
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
}
