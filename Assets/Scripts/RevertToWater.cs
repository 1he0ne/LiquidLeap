using UnityEngine;

public class RevertToWater: MonoBehaviour
{
    public GameObject WaterPrefab;

    public bool turnToWater;
    void OnDestroy()
    {
        if (turnToWater)
        {
            var water = Instantiate(WaterPrefab, transform.position, transform.rotation);
            Destroy(water, StaticConstants.WaterDestroyTime);
        }
    }
}
