using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;  // ����ģʽ

    private Queue<Task> taskQueue = new Queue<Task>();  // ���д洢����
    public GameObject taskUIPrefab;  // ������ʾ�����UIԤ����
    public Transform taskListParent;  // UI�����б�ĸ����壨������������

    void Start()
    {
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ������񵽶���
    public void AddTask(Task task)
    {
        taskQueue.Enqueue(task);
        UpdateTaskUI();
    }

    // ������������ɾ������
    public void RemoveTaskByName(string taskName)
    {
        Queue<Task> tempQueue = new Queue<Task>();

        // ����ƥ��������ݴ浽��ʱ����
        while (taskQueue.Count > 0)
        {
            Task currentTask = taskQueue.Dequeue();
            if (currentTask.taskName != taskName)
            {
                tempQueue.Enqueue(currentTask);
            }
        }

        // ����ʱ�����е��������¼��뵽������
        taskQueue = tempQueue;
        UpdateTaskUI();
    }

    // ��������UI
    private void UpdateTaskUI()
    {
        // ��յ�ǰUI
        foreach (Transform child in taskListParent)
        {
            Destroy(child.gameObject);
        }

        // ��ʾ��������е���������
        foreach (Task task in taskQueue)
        {
            GameObject newTaskUI = Instantiate(taskUIPrefab, taskListParent);
            TaskUI taskUIComponent = newTaskUI.GetComponent<TaskUI>();
            taskUIComponent.SetTask(task.taskName, task.taskDescription);
        }
    }
}
