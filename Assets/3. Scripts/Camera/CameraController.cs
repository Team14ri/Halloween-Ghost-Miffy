using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("카메라 회전 속성")]
    [SerializeField] private float cameraMaxSpeed = 1000.0f;
    [Tooltip("카메라 회전의 좌우반전 여부")]
    [SerializeField] private bool reverseCameraRotation = true;

    [Space]

    [Header("카메라 줌 속성")]
    [SerializeField] private float cameraZoomSpeed = 10.0f;
    [Tooltip("줌에 걸리는 시간 (높을수록 부드러워짐)")]
    [SerializeField] private float cameraZoomTime = 0.05f;
    [Tooltip("줌 최소 거리")]
    [SerializeField] private float cameraZoomMinLength = 1.0f;
    [Tooltip("줌 최대 거리")]
    [SerializeField] private float cameraZoomMaxLength = 12.0f;

    private CinemachineFreeLook _cinemachineFreeLook;
    private float targetOrthographicSize;
    private float lerpTime = 0;

    private void Awake()
    {
        CinemachineCore.GetInputAxis = GetInputAxis;
    }

    private void Start()
    {
        _cinemachineFreeLook = this.GetComponent<CinemachineFreeLook>();
        _cinemachineFreeLook.m_XAxis.m_MaxSpeed = cameraMaxSpeed;
        targetOrthographicSize = _cinemachineFreeLook.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        Zoom();
    }

    private float GetInputAxis(string axis)
    {
        if (!Input.GetMouseButton(0))
        {
            return 0;
        }

        var input = UnityEngine.Input.GetAxis(axis);
        return reverseCameraRotation ? -input : input;
    }

    private void Zoom()
    {
        var input = Input.GetAxis("Mouse ScrollWheel");
        
        if (Math.Abs(input) > 0)
        {
            // 카메라 줌이 변화할 값
            var zoomDelta = input * cameraZoomSpeed;

            // target생성, 최소~최대 줌 범위내에 있는지 확인
            targetOrthographicSize -= zoomDelta;
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, cameraZoomMinLength, cameraZoomMaxLength);
            lerpTime = 0;
        }

        // 현재 카메라와 target의 orthographic size가 다르다면 업데이트
        if (!Mathf.Approximately(_cinemachineFreeLook.m_Lens.OrthographicSize, targetOrthographicSize))
        {
            lerpTime += Time.deltaTime;
        }

        // 총 걸리는 시간 ~ 현재 경과한 시간 비율
        var percentComplete = lerpTime / cameraZoomTime;

        // 현재 카메라와 target의 orthographic size 보간
        _cinemachineFreeLook.m_Lens.OrthographicSize = Mathf.Lerp(
            _cinemachineFreeLook.m_Lens.OrthographicSize,
            targetOrthographicSize,
            percentComplete);
    }
}