using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    public static SceneManagerCustom instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("한 씬에 SceneManagerCustom가 여러 개 있습니다.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void LoadScene(string sceneName, int exitPortalNum)
    {
        StartCoroutine(LoadSceneAsync(sceneName, exitPortalNum));
    }
    
    private IEnumerator LoadSceneAsync(string sceneName, int exitPortalNum)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 씬 로드가 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("로딩 진행률: " + (progress * 100) + "%");

            yield return null;
        }

        Debug.Log("씬 로드 완료");
        PortalManager.instance.SceneChange(sceneName, exitPortalNum);
    }
}