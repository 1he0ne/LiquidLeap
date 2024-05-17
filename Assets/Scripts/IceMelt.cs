using UnityEngine;

public class IceMelt : MonoBehaviour
{
    public GameObject WaterPrefab;
    void OnDestroy()
    {
        var water = Instantiate(WaterPrefab, transform.position, transform.rotation);
        // the spawned particles should not extend their lifetime just when being frozen
        Destroy(water,  StaticConstants.WaterDestroyTime - StaticConstants.IceMeltTime); 
    }
}
