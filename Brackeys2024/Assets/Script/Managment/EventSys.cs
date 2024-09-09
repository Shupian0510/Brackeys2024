using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSys : MonoBehaviour{
    public bool Events; //����¼�����
    
    public float waitTime; //����¼����µ�����ѹ���ٶ�

    public float triggerTime; //����¼���ˢ���ٶ�

    public float triggerTimeVar; //����¼���ˢ�������

    public float StressSpeed; //����¼�������ѹ���ٶ�

    public float EventPossibility; //����¼��ĸ���

    public Button eventButton; //���ʱ�� ���� �İ�ť  

    public StressManage StressManage; //����ѹ��������
    void Start() {
        this.Events = false;
        Invoke("eventTrigger", waitTime);
        Invoke("randEvent", triggerTime);
    }

    private void randEvent() {
        if (!Events) {
            Events = Random.value > 1f-EventPossibility;
            Debug.Log("RAND EVENT");
        }
        Invoke("randEvent", triggerTime);
        triggerTime += Random.Range(-triggerTimeVar, triggerTimeVar);
    }

    public void eventTrigger() {
        if (Events)
        {
            eventButton.image.color = Color.red;
            StressManage.StressIncrease(StressSpeed);
            //Debug.Log("StressIncreased");
        }
        else {
            eventButton.image.color = Color.green;
        }
        
        Invoke("eventTrigger", waitTime);
    }

    public void OnbuttonDown(){
        Events = false;
    }
}
