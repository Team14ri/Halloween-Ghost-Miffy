using UnityEngine;

public class Portal : MonoBehaviour
{
    public int portalNum;
    [Space]
    [Header("ex) MiffyHouse씬의 1번 포탈로 이동시키고 싶다면?")]
    [Header("-> exitSceneName = MiffyHouse / exitPortalNum = 1")]
    [Space]
    public string exitSceneName;
    public int exitPortalNum = -1;

    private void Start()
    {
        PortalManager.Instance.PortalDictionary[portalNum] = this;
    }

    public void LoadScene()
    {
        SceneTransitionController.Instance.IrisClose(() =>
        {
            SceneManagerCustom.Instance.LoadScene(exitSceneName, exitPortalNum);
        });
    }
}