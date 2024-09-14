using UnityEngine;

public class MedManager : MonoBehaviour
{
    public static MedManager Instance;
    private void Awake() => Instance = this;

    // ����ҩƷ����ĸ���
    public Transform medicineParent; // ҩƷ�ĸ�����
    public float respawnTime = 5f;   // ҩƷ��ˢ��ʱ����

    private Transform[] medicines;    // �洢�����Ӷ���ҩƷ��
    public float calmvalue = 0.3f;

    void Start()
    {
        
        
        // ��ȡ�������µ�����ҩƷ���Ӷ���
        GetAllMedicines();

        // ��ʼ��ʱ������ҩƷ��Ϊ�Ǽ���״̬
        SetAllMedicinesActive(false);

        // ��ʼ��ʱ�����״����ˢ��ҩƷ
        //Invoke(nameof(RespawnMedicine), respawnTime);
    }

    // ��ȡ�������µ������Ӷ���
    void GetAllMedicines()
    {
        // ��ȡ�������µ�����ֱ���Ӷ���
        medicines = medicineParent.GetComponentsInChildren<Transform>(true);

        // ȥ��������������ã�ֻ�����Ӷ���
        medicines = System.Array.FindAll(medicines, t => t != medicineParent);
    }

    // ������ҩƷ����Ϊ�����Ǽ���
    void SetAllMedicinesActive(bool active)
    {
        foreach (var medicine in medicines)
        {
            medicine.gameObject.SetActive(active);
        }
    }

    // ���øú���ʹ��ĳ��ҩƷ��������
    public void UseMedicine()
    {
        StressManager.Instance.CalmDown(calmvalue);

        // ��ʼ����ˢ�µļ�ʱ
        Invoke(nameof(RespawnMedicine), respawnTime);
    }

    // �������ˢ��ҩƷ
    public void RespawnMedicine()
    {
        // ���ѡ��һ��δ�����ҩƷ����ˢ��
        Transform medicineToRespawn = null;
        do
        {
            medicineToRespawn = medicines[Random.Range(0, medicines.Length)];
        }
        while (medicineToRespawn.gameObject.activeSelf); // ȷ��ѡ�����δ�����ҩƷ

        // ����ѡ�е�ҩƷ
        medicineToRespawn.gameObject.SetActive(true);

        
    }
}
