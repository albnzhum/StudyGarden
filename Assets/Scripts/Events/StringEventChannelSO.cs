using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(fileName = "String Event Channel", menuName = "Events/String Event Channel")]
    public class StringEventChannelSO : ScriptableObject
    {
        public event UnityAction<string, string> OnEventRaised;

        public void RaiseEvent(string text, string text2)
        {
            if (OnEventRaised != null)
            {
                OnEventRaised.Invoke(text, text2);
            }
            else
            {
                Debug.Log("There is no subscribers!");
            }
        }
    }
}