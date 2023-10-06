using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public static PortalManager instance { get; private set; }
    public GameObject player { get; private set; }
    
    [Tooltip("포탈이 활성화 되는 거리 (반지름)")]
    public float portalActivationRange = 5.0f;
    private Dictionary<int, Portal> portalDictionary = new Dictionary<int, Portal>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("한 씬에 PortalManager가 여러 개 있어 삭제합니다.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
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
        FindPortals();
        
        Vector3 exitPortalPosition = GetPortalPosition(exitPortalNum);
        SetPlayerPosition(exitPortalPosition);
    }
    void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }
    void FindPortals()
    {
        portalDictionary.Clear();
        GameObject[] portalObjects = GameObject.FindGameObjectsWithTag("Portal");

        foreach (var portalObject  in portalObjects)
        {
            Portal portal = portalObject.GetComponent<Portal>();
            if (portal != null)
            {
                portalDictionary[portal.portalNum] = portal;
            }
        }
    }
    private Vector3 GetPortalPosition(int portalNum)
    {
        if (!portalDictionary.ContainsKey(portalNum))
        {
            Debug.LogError("포탈을 찾지 못했습니다.");
            return Vector3.zero;
        }

        Vector3 portalPosition = portalDictionary[portalNum].transform.position;
        return portalPosition;
    }
    private void SetPlayerPosition(Vector3 portalPos)
    {
        player.transform.position = portalPos;
    }
}