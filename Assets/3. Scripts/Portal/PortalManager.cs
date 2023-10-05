using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public static PortalManager instance { get; private set; }
    public GameObject player { get; private set; }
    public float portalActivationRange = 5.0f;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("한 씬에 PortalManager가 여러 개 있습니다.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            FindPlayer();
        }
    }

    void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }

    // 함수명 변경이 필요할듯
    public void SceneChange(string sceneName, int exitPortalNum)
    {
        if (sceneName == null || exitPortalNum == -1)
        {
            Debug.Log("포탈의 exit 정보가 잘못됐습니다.");
            return;
        }
        
        FindPlayer();

        Vector3 exitPortalPos = FindPortalPosition(exitPortalNum);
        SetPlayerPosition(exitPortalPos);
    }

    private Vector3 FindPortalPosition(int portalNum)
    {
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
        
        foreach (var cur in portals)
        {
            Debug.Log("포탈 찾는중" + cur.GetComponent<Portal>().portalNum);

            if (cur.GetComponent<Portal>().portalNum == portalNum)
            {
                Debug.Log("포탈 찾음");
                return cur.transform.position;
            }
        }

        Debug.Log("못찾음" + SceneManager.GetActiveScene().name);
        return Vector3.zero;
    }

    private void SetPlayerPosition(Vector3 portalPos)
    {
        player.transform.position = portalPos;
    }
}