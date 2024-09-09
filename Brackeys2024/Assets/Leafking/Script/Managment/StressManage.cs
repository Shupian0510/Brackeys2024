using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressManage : MonoBehaviour{
    public float StressIndex = 1; //压力系数
    public float IncTime; //压力上升、下降的时间
    private Animator ani;
    
    public Scrollbar StressProcess;

    void Start(){
        Invoke("timeAlongStress", IncTime);
    }

    void timeAlongStress(){
        //.Log("StressIncreased by -" + .01f);
        Invoke("timeAlongStress", IncTime);
        StressIncrease(-.01f);
    }//压力根据时间进行下降
    
    public void StressIncrease(float increase){
        this.StressIndex += increase;
        StressProcess.size = this.StressIndex;
        StressProcess.image.color = new Color(StressIndex * 2,2 - 2 * StressIndex,0);
    }//刷新压力显示UI

    public void StressedOut() {
        if (this.StressIndex > 0.8) {
            ani.Play("BloodyFlush");
        }
    }
}
