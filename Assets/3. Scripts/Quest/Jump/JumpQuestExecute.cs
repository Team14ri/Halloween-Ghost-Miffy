using UnityEngine;
using UnityEngine.Events;

public class JumpQuestExecute : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float increase;
    [SerializeField, Range(0f, 100f)] private float decrease;
    [SerializeField, Range(0f, 100f)] private float targetValue = 100f;
    [SerializeField] private UnityEvent unityEvent;

    public void Execute()
    {
        PlayerQuest.Instance.StartJumpQuest(increase, decrease, targetValue, Done);
    }

    private void Done()
    {
        unityEvent?.Invoke();
    }
}
