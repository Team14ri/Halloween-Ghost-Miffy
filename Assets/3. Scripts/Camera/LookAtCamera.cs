using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera Cam { get; set; }

    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = false;

    private void Start()
    {
        Cam = Camera.main;
        UpdateRotation();
    }

    private void Update()
    {
        UpdateRotation();
    }
    
    private void UpdateRotation()
    {
        if (!followX && !followY)
            return;
        
        float cameraYRotation = followX ? Cam.transform.eulerAngles.y : transform.eulerAngles.y;
        float cameraXRotation = followY ? Cam.transform.eulerAngles.x : transform.eulerAngles.x;
        
        transform.rotation = Quaternion.Euler(cameraXRotation, cameraYRotation, transform.eulerAngles.z);
    }
}