using System;
using UnityEngine;

public class LookAtCameraYOnly : MonoBehaviour
{
    public static Camera Cam { get; private set; }

    private void Start()
    {
        Cam = Camera.main;
    }

    private void FixedUpdate()
    {
        float cameraYRotation = Cam.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, cameraYRotation, transform.eulerAngles.z);
    }
}