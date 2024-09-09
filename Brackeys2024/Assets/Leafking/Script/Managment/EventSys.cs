using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSys : MonoBehaviour{
    public bool Events; //随机事件发生
    
    public float waitTime; //随机事件导致的增长压力速度

    public float triggerTime; //随机事件的刷新速度

    public float triggerTimeVar; //随机事件的刷新随机度

    public float StressSpeed; //随机事件增长的压力速度

    public float EventPossibility; //随机事件的概率

    public Button eventButton; //随机时间 测试 的按钮  

    public StressManage StressManage; //调用压力管理器
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
