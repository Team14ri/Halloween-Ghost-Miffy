using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    public static SceneManagerCustom instance { get; private set; }
    private Cutout cutout;
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("한 씬에 SceneManagerCustom가 여러 개 있어 삭제합니다.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            FindCutout();
        }
    }

    // 씬 전환시 호출될 최종 함수
    public void LoadScene(string sceneName, int exitPortalNum)
    {
        // 기존 포탈 정보를 초기화
        PortalManager.Instance.PortalDictionary.Clear();
        StartCoroutine(LoadSceneCoroutine(sceneName, exitPortalNum));
    }
    
    private IEnumerator LoadSceneCoroutine(string sceneName, int exitPortalNum)
    {
        yield return StartCoroutine(cutout.FadeOutCoroutine());
        
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
            Debug.Log("씬 로드 완료");
            PortalManager.Instance.TeleportPlayerToExitPortal(sceneName, exitPortalNum);
            
            FindCutout();
            yield return StartCoroutine(cutout.FadeInCoroutine());
        }
        else
        {
            Debug.LogError("씬 로드 중 예외 발생 또는 씬 활성화가 중지되었습니다.");
        }


    }

    private void FindCutout()
    {
        cutout = GameObject.FindObjectOfType<Cutout>();

        if (cutout == null)
        {
            Debug.Log("Cutout 스크립트를 가진 객체를 찾지 못했습니다.");
        }
    }
}