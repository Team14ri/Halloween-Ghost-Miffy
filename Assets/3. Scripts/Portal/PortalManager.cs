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
        bool spawnFacingLeft = CheckSpawnFacingLeft(exitPortalNum);
        SetPlayerPosition(exitPortalPosition, spawnFacingLeft);
    }
    
    void FindPlayer()
    {
        Player = GameObject.FindWithTag("Player");
    }
    
    private Vector3 GetPortalPosition(int portalNum)
    {
        if (!PortalDictionary.ContainsKey(portalNum))
        {
            Debug.LogError($"포탈 {portalNum}을 찾을 수 없습니다.");
            return Vector3.zero;
        }

        Vector3 portalPosition = PortalDictionary[portalNum].GetSpawnPoint().position;
        return portalPosition;
    }
    
    private bool CheckSpawnFacingLeft(int portalNum)
    {
        if (!PortalDictionary.ContainsKey(portalNum))
        {
            Debug.LogError($"포탈 {portalNum}을 찾을 수 없습니다.");
            return false;
        }

        return PortalDictionary[portalNum].CheckSpawnFacingLeft();
    }
    
    private void SetPlayerPosition(Vector3 portalPos, bool facingLeft)
    {
        if (Player == null)
            return;
        
        Player.transform.position = portalPos;
        PlayerController.Instance.ChangePlayerFacingImmediately(facingLeft ? -1 : 1);
    }
}