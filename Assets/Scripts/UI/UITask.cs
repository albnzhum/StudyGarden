using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITask : MonoBehaviour
{
    [SerializeField] private TMP_Text nameTask;

    private Task.Task task = default;
    public Task.Task Task => task;

    public void SetTask(Task.Task taskToSet)
    {
        task = taskToSet;

        SetTaskUI();
    }

    public void SetTaskUI()
    {
        nameTask.text = task.title;
    }
}
