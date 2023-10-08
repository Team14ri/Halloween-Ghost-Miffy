using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public static PortalManager instance { get; private set; }
    public GameObject player { get; private set; }
    
    [Tooltip("포탈이 활성화 되는 거리 (반지름)")]
    public float portalActivationRange = 5.0f;
    public readonly Dictionary<int, Portal> PortalDictionary = new();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("한 씬에 PortalManager가 여러 개 있어 삭제합니다.");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            FindPlayer();
        }
    }

    public void TeleportPlayerToExitPortal(string sceneName, int exitPortalNum)
    {
        if (sceneName == null || exitPortalNum == -1)
        {
            Debug.Log("포탈의 exit 정보가 잘못됐습니다.");
            return;
        }

        FindPlayer();
        Vector3 exitPortalPosition = GetPortalPosition(exitPortalNum);
        SetPlayerPosition(exitPortalPosition);
    }
    
    void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
        
        if (player == null)
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다.");
        }
    }
    
    private Vector3 GetPortalPosition(int portalNum)
    {
        if (!PortalDictionary.ContainsKey(portalNum))
        {
            Debug.LogError($"포탈 {portalNum}을 찾을 수 없습니다.");
            return Vector3.zero;
        }

        Vector3 portalPosition = PortalDictionary[portalNum].transform.position;
        return portalPosition;
    }
    
    private void SetPlayerPosition(Vector3 portalPos)
    {
        player.transform.position = portalPos;
    }
}