using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    public static SceneManagerCustom Instance { get; private set; }
    
    public string LastVisitPortalLocation
    {
        get => PlayerPrefs.GetString("LastVisitPortal@Location", "Cemetery");  
        set => PlayerPrefs.SetString("LastVisitPortal@Location", value);
    }
    public int LastVisitPortalID
    {
        get => PlayerPrefs.GetInt("LastVisitPortal@ID", 0);  
        set => PlayerPrefs.SetInt("LastVisitPortal@ID", value);
    }
    
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
        }
    }

    // 씬 전환시 호출될 최종 함수
    public void LoadScene(string sceneName, int exitPortalNum)
    {
        // 기존 포탈 정보를 초기화
        PortalManager.Instance.PortalDictionary.Clear();
        LastVisitPortalLocation = sceneName;
        LastVisitPortalID = exitPortalNum;
        StartCoroutine(LoadSceneCoroutine(sceneName, exitPortalNum));
    }
    
    public void LoadTitleScene()
    {
        SceneTransitionController.Instance.IrisClose(() =>
        {
            LoadScene("Title", 1);
        });
    }
    
    private IEnumerator LoadSceneCoroutine(string sceneName, int exitPortalNum)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 씬 로드가 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            // float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            // Debug.Log("로딩 진행률: " + (progress * 100) + "%");
            yield return null;
        }

        if (asyncLoad.isDone && asyncLoad.allowSceneActivation)
        {
            PortalManager.Instance.TeleportPlayerToExitPortal(sceneName, exitPortalNum);
            BGMPlayer.Instance.ChangeBGM(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.LogError("씬 로드 중 예외 발생 또는 씬 활성화가 중지되었습니다.");
        }
    }
}