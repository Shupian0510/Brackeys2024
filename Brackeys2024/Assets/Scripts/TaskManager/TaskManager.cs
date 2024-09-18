using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;  // 单例模式

    private Queue<Task> taskQueue = new Queue<Task>();  // 队列存储任务
    public GameObject taskUIPrefab;  // 用于显示任务的UI预制体
    public Transform taskListParent;  // UI任务列表的父物体（用于排列任务）

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

    // 添加任务到队列
    public void AddTask(Task task)
    {
        taskQueue.Enqueue(task);
        UpdateTaskUI();
    }

    // 根据任务名称删除任务
    public void RemoveTaskByName(string taskName)
    {
        Queue<Task> tempQueue = new Queue<Task>();

        // 将不匹配的任务暂存到临时队列
        while (taskQueue.Count > 0)
        {
            Task currentTask = taskQueue.Dequeue();
            if (currentTask.taskName != taskName)
            {
                tempQueue.Enqueue(currentTask);
            }
        }

        // 将临时队列中的任务重新加入到主队列
        taskQueue = tempQueue;
        UpdateTaskUI();
    }

    // 更新任务UI
    private void UpdateTaskUI()
    {
        // 清空当前UI
        foreach (Transform child in taskListParent)
        {
            Destroy(child.gameObject);
        }

        // 显示任务队列中的所有任务
        foreach (Task task in taskQueue)
        {
            GameObject newTaskUI = Instantiate(taskUIPrefab, taskListParent);
            TaskUI taskUIComponent = newTaskUI.GetComponent<TaskUI>();
            taskUIComponent.SetTask(task.taskName, task.taskDescription);
        }
    }
}
