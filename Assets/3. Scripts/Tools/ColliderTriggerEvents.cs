using UnityEngine;
using UnityEngine.Events;

public class ColliderTriggerEvents : MonoBehaviour
{
    [SerializeField] private string filterTag;
    [SerializeField] private UnityEvent onEnterEvent;
    [SerializeField] private UnityEvent onExitEvent;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(filterTag) == false)
            return;
        onEnterEvent.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(filterTag) == false)
            return;
        onExitEvent.Invoke();
    }
}
