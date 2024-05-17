using UnityEngine;

public class RevertToWater: MonoBehaviour
{
    public GameObject WaterPrefab; // TODO: should be loaded from Resources

    public bool turnToWater; // maybe should be set with a setter?
    void OnDestroy()
    {
        if ( turnToWater )
        {
            var water = Instantiate(WaterPrefab, transform.position, transform.rotation);
            // the spawned particles should not extend their lifetime just when being heated
            Destroy(water, StaticConstants.WaterDestroyTime - StaticConstants.SteamCondenseTime);
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
