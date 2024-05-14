using UnityEngine;

public class IceMelt : MonoBehaviour
{
    public GameObject WaterPrefab;
    void OnDestroy()
    {
        var water = Instantiate(WaterPrefab, transform.position, transform.rotation);
        Destroy(water, StaticConstants.WaterDestroyTime);
    }
}
