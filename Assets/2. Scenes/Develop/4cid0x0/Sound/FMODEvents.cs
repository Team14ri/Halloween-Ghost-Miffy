using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    //[field:Header("Test BGM1")]
    public EventReference testBGM1;
    
    //[field:Header("Test BGM2")]
    public EventReference testBGM2;
    
    public static FMODEvents Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("한 씬에 FMODEvents가 여러 개 있습니다.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
