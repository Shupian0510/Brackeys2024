using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressManage : MonoBehaviour{
    public float StressIndex = 1; //ѹ��ϵ��
    public float IncTime; //ѹ���������½���ʱ��
    private Animator ani;
    
    public Scrollbar StressProcess;

    void Start(){
        Invoke("timeAlongStress", IncTime);
    }

    void timeAlongStress(){
        //.Log("StressIncreased by -" + .01f);
        Invoke("timeAlongStress", IncTime);
        StressIncrease(-.01f);
    }//ѹ������ʱ������½�
    
    public void StressIncrease(float increase){
        this.StressIndex += increase;
        StressProcess.size = this.StressIndex;
        StressProcess.image.color = new Color(StressIndex * 2,2 - 2 * StressIndex,0);
    }//ˢ��ѹ����ʾUI

    public void StressedOut() {
        if (this.StressIndex > 0.8) {
            ani.Play("BloodyFlush");
        }
    }
}
