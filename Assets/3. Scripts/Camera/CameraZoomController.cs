using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public static CameraZoomController Instance;
    
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float zoomInTime = 1f;
    [SerializeField] private float zoomOutTime = 1f;
    
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
        
        initialYAxisValue = freeLookCamera.m_YAxis.Value;
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

            freeLookCamera.m_YAxis.Value = Mathf.Lerp(initialYAxisValue, 0, percentage);

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

            freeLookCamera.m_YAxis.Value = Mathf.Lerp(0, initialYAxisValue, percentage);

            yield return null;
        }

        freeLookCamera.m_YAxis.Value = initialYAxisValue;
    }
}
