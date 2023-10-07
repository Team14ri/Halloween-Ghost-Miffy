using UnityEngine;

public class Portal : MonoBehaviour
{
    public int portalNum;
    [Space]
    [Header("ex) MiffyHouse씬의 1번 포탈로 이동시키고 싶다면?")]
    [Header("-> exitSceneName = MiffyHouse / exitPortalNum = 1")]
    [Space]
    public string exitSceneName = null;
    public int exitPortalNum = -1;

    private void Start()
    {
        PortalManager.instance.portalDictionary[portalNum] = this;
    }

    private void Update()
    {
        // 포탈마다 Update문을 돌리지 않고, 플레이어가 포탈을 탐지하게 변경하면 좋을것 같습니다.
        if (Input.GetKeyDown(KeyCode.Space) && IsPlayerCloseToPortal())
        {
            SceneManagerCustom.instance.LoadScene(exitSceneName, exitPortalNum);
        }
    }

    public bool IsPlayerCloseToPortal()
    {
        Vector3 portalPos = this.transform.position;
        Vector3 playerPos = PortalManager.instance.player.transform.position;
        Vector3 difference = portalPos - playerPos;

        float distance = difference.magnitude;

        // 플레이어와의 거리가 설정된 거리보다 작거나 같다면 true
        return distance <= PortalManager.instance.portalActivationRange;
    }
}