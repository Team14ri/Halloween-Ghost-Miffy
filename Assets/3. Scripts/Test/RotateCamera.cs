using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5.0f;
    
    private void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.y += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
