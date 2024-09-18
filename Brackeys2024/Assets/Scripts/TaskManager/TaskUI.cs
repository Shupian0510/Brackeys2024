using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    public TMP_Text taskNameText;  // 显示任务名称
    public TMP_Text taskDescriptionText;  // 显示任务描述

    public void SetTask(string name, string description)
    {
        taskNameText.text = name;
        taskDescriptionText.text = description;
    }
}
