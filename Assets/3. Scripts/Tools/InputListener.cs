using UnityEngine;
using UnityEngine.Events;

public class InputListener : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private UnityEvent inputEvent;
    private void Update()
    {
        if (key == KeyCode.None)
            return;
        
        if (Input.GetKeyDown(key))
        {
            inputEvent?.Invoke();
        }
    }
}
