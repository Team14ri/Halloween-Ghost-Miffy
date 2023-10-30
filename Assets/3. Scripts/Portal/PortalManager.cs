using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public static PortalManager Instance { get; private set; }

    private GameObject Player { get; set; }
    
    public readonly Dictionary<int, Portal> PortalDictionary = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(transform.gameObject);
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
        Player = GameObject.FindWithTag("Player");
        
        if (Player == null)
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
        Player.transform.position = portalPos;
    }
}