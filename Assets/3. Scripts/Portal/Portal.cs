using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    /* Portal 스크립트를 적용시 주의사항
     * 스크립트가 적용될 오브젝트의 이름의 끝에 숫자가 들어가야 합니다.
     * ex) Portal 1, Portal 2...
     */
    [HideInInspector] public int portalNum;
    [Tooltip("이동시킬 씬 이름   (ex. MiffyHouse.unity로 이동시킨다면, MiffyHouse 입력)")]
    public string exitSceneName = null;
    [Tooltip("이동시킬 포탈 번호   (ex. 마을 씬의 Portal 1로 이동시킨다면, 1 입력")]
    public int exitPortalNum = -1;

    private void Awake()
    {
        portalNum = int.Parse(this.gameObject.name.Replace("Portal ", ""));
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
        Vector3 different = portalPos - playerPos;

        float distance = different.magnitude;

        // 플레이어와의 거리가 설정된 거리보다 작거나 같다면 true
        return distance <= PortalManager.instance.portalActivationRange;
    }
}