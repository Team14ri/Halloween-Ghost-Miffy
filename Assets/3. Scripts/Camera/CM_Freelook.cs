using UnityEngine;
using Cinemachine;

public class CM_Freelook : MonoBehaviour
{
    private CinemachineFreeLook _cinemachineFreeLook;

    [SerializeField] private float dragMinDistance = 100.0f;

    [SerializeField] private float cameraMaxSpeed = 1000.0f;
    [SerializeField] private float cameraZoomSpeed = 2000.0f;
    [SerializeField] private float cameraZoomMaxSpeed;
    [SerializeField] private float cameraZoomMinLength = 1.0f;
    [SerializeField] private float cameraZoomMaxLength = 12.0f;

    private void Awake()
    {
        CinemachineCore.GetInputAxis = GetInputAxis;
    }

    private void Start()
    {
        _cinemachineFreeLook = this.GetComponent<CinemachineFreeLook>();
        _cinemachineFreeLook.m_XAxis.m_MaxSpeed = cameraMaxSpeed;
    }

    private void Update()
    {
        Zoom();
    }

    private float GetInputAxis(string axis)
    {
        if (Input.GetMouseButton(0))
        {
            float input = UnityEngine.Input.GetAxis(axis);

            return input;
        }

        return 0;
    }

    private void Zoom()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        float input = scrollWheel * cameraZoomSpeed * Time.deltaTime;

        if (_cinemachineFreeLook.m_Lens.OrthographicSize - input > cameraZoomMinLength
            && _cinemachineFreeLook.m_Lens.OrthographicSize - input < cameraZoomMaxLength)
        {
            _cinemachineFreeLook.m_Lens.OrthographicSize -= input;
        }
    }
}