using System;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    [SerializeField] private PlayerJumpQuest jumpQuest;
    
    public static PlayerQuest Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartJumpQuest(float increase, float decrease, float targetValue, Action doneAction)
    {
        jumpQuest.gameObject.SetActive(true);
        jumpQuest.Init(increase, decrease, targetValue, doneAction);
    }
}
