using UnityEngine;

public class MedManager : MonoBehaviour
{
    public static MedManager Instance;
    private void Awake() => Instance = this;

    // 定义药品对象的父级
    public Transform medicineParent; // 药品的父对象
    public float respawnTime = 5f;   // 药品的刷新时间间隔

    private Transform[] medicines;    // 存储所有子对象（药品）
    public float calmvalue = 0.3f;

    void Start()
    {
        
        
        // 获取父对象下的所有药品（子对象）
        GetAllMedicines();

        // 初始化时将所有药品设为非激活状态
        SetAllMedicinesActive(false);

        // 开始计时器，首次随机刷新药品
        //Invoke(nameof(RespawnMedicine), respawnTime);
    }

    // 获取父对象下的所有子对象
    void GetAllMedicines()
    {
        // 获取父对象下的所有直接子对象
        medicines = medicineParent.GetComponentsInChildren<Transform>(true);

        // 去掉父对象本身的引用，只保留子对象
        medicines = System.Array.FindAll(medicines, t => t != medicineParent);
    }

    // 将所有药品设置为激活或非激活
    void SetAllMedicinesActive(bool active)
    {
        foreach (var medicine in medicines)
        {
            medicine.gameObject.SetActive(active);
        }
    }

    // 调用该函数使用某个药品并禁用它
    public void UseMedicine()
    {
        StressManager.Instance.CalmDown(calmvalue);

        // 开始重新刷新的计时
        Invoke(nameof(RespawnMedicine), respawnTime);
    }

    // 重新随机刷新药品
    public void RespawnMedicine()
    {
        // 随机选择一个未激活的药品进行刷新
        Transform medicineToRespawn = null;
        do
        {
            medicineToRespawn = medicines[Random.Range(0, medicines.Length)];
        }
        while (medicineToRespawn.gameObject.activeSelf); // 确保选择的是未激活的药品

        // 激活选中的药品
        medicineToRespawn.gameObject.SetActive(true);

        
    }
}
