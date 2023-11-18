using FMOD.Studio;
using UnityEngine;

public class BGMCrossObject : MonoBehaviour
{
    private Transform player;
    private EventInstance eventInstance;
    public float minOffset = 5f;
    public float maxOffset = 10f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        if (!eventInstance.isValid())
        {
            eventInstance = BGMPlayer.Instance.bgmInstance;
        }
        
        float distance = Vector3.Distance(transform.position, player.position);
        float normalizedDistance = Mathf.Clamp01((distance - minOffset) / (maxOffset - minOffset));
        eventInstance.setParameterByName("trackVol", 1 - normalizedDistance);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        Gizmos.DrawWireSphere(transform.position, minOffset);
        Gizmos.DrawWireSphere(transform.position, maxOffset);
    }
}