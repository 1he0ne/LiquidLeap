using Unity.VisualScripting;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Ground" )
        {
            Destroy(gameObject);
        }
    }
}
