using UnityEngine;

public class RevertToWater: MonoBehaviour
{
    public GameObject WaterPrefab;
    void OnDestroy()
    {
        var water = Instantiate(WaterPrefab, transform.position, transform.rotation);
        Destroy(water, StaticConstants.WaterDestroyTime);
    }
}
