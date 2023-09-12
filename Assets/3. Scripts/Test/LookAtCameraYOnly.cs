using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraYOnly : MonoBehaviour
{
    private void Update()
    {
        Vector3 directionToCamera = transform.position - Camera.main.transform.position;
        
        directionToCamera.y = 0;
        
        transform.forward = directionToCamera.normalized;
    }
}
