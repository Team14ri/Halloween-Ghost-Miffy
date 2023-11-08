using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    public static VirtualCameraController Instance;
    
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    
    [SerializeField] private float zoomInTime = 0.5f;
    [SerializeField] private AnimationCurve ZoomInAccelerationCurve = new(new Keyframe(0, 0), new Keyframe(1, 1));
    [SerializeField] private float zoomOutTime = 1f;
    
    [SerializeField] private AnimationCurve ZoomOutAccelerationCurve = new(new Keyframe(0, 0), new Keyframe(1, 1));

    private Coroutine _zoomRoutine;
    private float initialYAxisValue;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (freeLookCamera == null)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("PlayerViewPoint");
        if (player)
        {
            freeLookCamera.Follow = player.transform;
            freeLookCamera.LookAt = player.transform;
        }
        
        initialYAxisValue = freeLookCamera.m_YAxis.Value;
    }

    public void SetXAxis(float xAxis)
    {
        freeLookCamera.m_XAxis.Value = xAxis;
    } 
    
    public float GetXAxis()
    {
        return freeLookCamera.m_XAxis.Value;
    }

    public void ZoomIn()
    {
        if (freeLookCamera == null)
            return;
        
        this.EnsureCoroutineStopped(ref _zoomRoutine);
        _zoomRoutine = StartCoroutine(ZoomInProcess(zoomInTime));
    }

    public void ZoomOut()
    {
        if (freeLookCamera == null)
            return;
        
        this.EnsureCoroutineStopped(ref _zoomRoutine);
        _zoomRoutine = StartCoroutine(ZoomOutProcess(zoomOutTime));
    }
    
    private IEnumerator ZoomInProcess(float zoomInTime)
    {
        float elapsedTime = 0;

        while (elapsedTime < zoomInTime)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / zoomInTime;

            float curveValue = ZoomInAccelerationCurve.Evaluate(percentage);
            freeLookCamera.m_YAxis.Value = Mathf.Lerp(initialYAxisValue, 0, curveValue);

            yield return null;
        }

        freeLookCamera.m_YAxis.Value = 0;
    }

    private IEnumerator ZoomOutProcess(float zoomOutTime)
    {
        float elapsedTime = 0;

        while (elapsedTime < zoomOutTime)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / zoomOutTime;

            float curveValue = ZoomOutAccelerationCurve.Evaluate(percentage);
            freeLookCamera.m_YAxis.Value = Mathf.Lerp(0, initialYAxisValue, curveValue);

            yield return null;
        }

        freeLookCamera.m_YAxis.Value = initialYAxisValue;
    }
}
