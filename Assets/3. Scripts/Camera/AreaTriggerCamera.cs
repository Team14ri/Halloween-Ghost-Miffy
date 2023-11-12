using Cinemachine;
using UnityEngine;

public class AreaTriggerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float autoEnterOffset = 2f;
    [SerializeField] private float autoExitTime = 5f;
    
    public void AutoEnterExit()
    {
        Invoke(nameof(OnEnter), autoEnterOffset);
        Invoke(nameof(OnExit), autoEnterOffset + autoExitTime);
    }
    
    public void OnEnter()
    {
        freeLookCamera.m_XAxis.Value = VirtualCameraController.Instance.GetXAxis();
        freeLookCamera.m_Priority = 11;
    }
    
    public void OnExit()
    {
        freeLookCamera.m_Priority = 9;
    }
}
