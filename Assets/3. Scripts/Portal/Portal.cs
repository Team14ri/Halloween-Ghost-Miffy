using Sirenix.OdinInspector;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [BoxGroup("Info"), SerializeField] private int portalNum;
    [BoxGroup("Info"), SerializeField] private Transform spawnPoint;

    [BoxGroup("Exit Info"), SerializeField] private string exitSceneName;
    [BoxGroup("Exit Info"), SerializeField] private int exitPortalNum = 1;
    
    private void Start()
    {
        PortalManager.Instance.PortalDictionary[portalNum] = this;
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    public void LoadScene()
    {
        SceneTransitionController.Instance.IrisClose(() =>
        {
            SceneManagerCustom.Instance.LoadScene(exitSceneName, exitPortalNum);
        });
    }
    
    public void LoadSceneWithoutIris()
    {
        if (BGMPlayer.Instance != null)
        {
            BGMPlayer.Instance.StopSound();
        }
        
        SceneManagerCustom.Instance.LoadScene(exitSceneName, exitPortalNum);
    }
    
    public void LoadLastVisitScene()
    {
        SceneTransitionController.Instance.IrisClose(() =>
        {
            SceneManagerCustom.Instance.LoadScene(SceneManagerCustom.Instance.LastVisitPortalLocation, SceneManagerCustom.Instance.LastVisitPortalID);
        });
    }
}