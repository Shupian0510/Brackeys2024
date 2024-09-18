using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    public TMP_Text taskNameText;  // ��ʾ��������
    public TMP_Text taskDescriptionText;  // ��ʾ��������

    public void SetTask(string name, string description)
    {
        taskNameText.text = name;
        taskDescriptionText.text = description;
    }
}
