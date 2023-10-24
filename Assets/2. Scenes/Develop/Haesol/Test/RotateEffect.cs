using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    public float rotationSpeed = 180.0f; // ȸ�� �ӵ�

    void Update()
    {
        // A Ű�� ������ Z�� ȸ���� 180���� ����
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 180);
        }
        // D Ű�� ������ Z�� ȸ���� 0���� ����
        else if (Input.GetKey(KeyCode.D))
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
        }
    }
}
