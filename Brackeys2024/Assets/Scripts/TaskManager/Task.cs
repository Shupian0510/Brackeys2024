[System.Serializable]
public class Task
{
    public string taskName;
    public string taskDescription;

    public Task(string name, string description)
    {
        taskName = name;
        taskDescription = description;
    }
}
