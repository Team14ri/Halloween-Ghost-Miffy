using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTriggerEvents : MonoBehaviour
{
    [SerializeField] private string filterTag;
    [SerializeField, Space(10)] private bool executeEnterAfterFrame;
    [SerializeField] private UnityEvent onEnterEvent;
    [SerializeField, Space(10)] private bool executeExitAfterFrame;
    [SerializeField] private UnityEvent onExitEvent;

    private IEnumerator ExecuteAfterFrame(UnityEvent unityEvent)
    {
        yield return null;
        unityEvent?.Invoke();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(filterTag) == false)
            return;
        
        if (executeEnterAfterFrame)
        {
            StartCoroutine(ExecuteAfterFrame(onEnterEvent));
            return;
        }
        
        onEnterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(filterTag) == false)
            return;
        
        if (executeExitAfterFrame)
        {
            StartCoroutine(ExecuteAfterFrame(onExitEvent));
            return;
        }

        onExitEvent?.Invoke();
    }
}
