using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    public static SceneManagerCustom Instance { get; private set; }
    
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