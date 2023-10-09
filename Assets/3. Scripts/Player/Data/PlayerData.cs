using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Object/Player Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private float movementSpeed = 5f;
    public float MovementSpeed => movementSpeed;
}
