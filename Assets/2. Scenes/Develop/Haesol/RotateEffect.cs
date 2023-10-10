using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    public float rotationSpeed = 180.0f; // 회전 속도

    void Update()
    {
        // A 키를 누르면 Z축 회전을 180도로 변경
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 180);
        }
        // D 키를 누르면 Z축 회전을 0도로 변경
        else if (Input.GetKey(KeyCode.D))
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
        }
    }
}
