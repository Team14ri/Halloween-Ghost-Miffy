using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Player Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private float movementSpeed = 5f;
    public float MovementSpeed => movementSpeed;
    
    [SerializeField] private float changeFacingDuration = 1f;
    public float ChangeFacingDuration => changeFacingDuration;
}
