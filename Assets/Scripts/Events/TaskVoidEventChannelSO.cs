using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(fileName = "Task Void Event Channel", menuName = "Events/Task Void Event Channel")]
    public class TaskVoidEventChannelSO : ScriptableObject
    {
        public event UnityAction<Task.Task> OnEventRaised = default;
        public void RaiseEvent(Task.Task task)
        {
            if (OnEventRaised != null)
            {
                OnEventRaised.Invoke(task);
            }
            else
            {
                Debug.LogError("There is no subscribers");
            }
        }
    }
}