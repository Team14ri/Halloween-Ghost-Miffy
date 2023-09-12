using UnityEngine;

public class LookAtCameraYOnly : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    private void Update()
    {
        float cameraYRotation = cam.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, cameraYRotation, transform.eulerAngles.z);
    }
}