using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float Force;
    [SerializeField] public GameObject WaterPrefab;
    [SerializeField] public GameObject IcePrefab;

    [SerializeField] public LayerMask FreezeRayStopLayers;
    [SerializeField] public LayerMask FreezeRayLayers;

    public Transform AimingPoint;

    public SpriteRenderer PlayerSprite;

    public GameObject Gun;
    private SpriteRenderer GunSprite;

    private Vector2 WorldMousePos;
    private Vector2 AimPos;
    private Vector2 AimDirectionNorm;


    private void Start()
    {
        GunSprite = Gun.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        WorldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        AimDirectionNorm = (WorldMousePos - (Vector2)transform.position).normalized;
        AimPos = AimingPoint.position;

        float angle = Mathf.Atan2(AimDirectionNorm.y, AimDirectionNorm.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector2 targetPosition = AimPos + (AimDirectionNorm * 0.5f);

        Gun.transform.position = targetPosition;
        Gun.transform.rotation = targetRotation;
  
        if ( Mathf.Abs(angle) > 90 )
        {
            GunSprite.flipY = true;
            PlayerSprite.flipX = true;
        }

        if ( Mathf.Abs(angle) < 90 )
        {
            GunSprite.flipY = false;
            PlayerSprite.flipX = false;
        }


        if ( Input.GetMouseButton(0) )
        {
            Shoot();
        }

        if ( Input.GetMouseButton(1) )
        {
            FreezeRay();
        }
    }

    void Shoot()
    {
        // Creates the water locally
        GameObject waterParticle = Instantiate(WaterPrefab, AimingPoint.position + (Vector3)(AimDirectionNorm * 0.5f), Quaternion.identity);  

        // Adds velocity to the bullet
        waterParticle.GetComponent<Rigidbody2D>().velocity = AimDirectionNorm * Force;
        Destroy(waterParticle.gameObject, StaticConstants.WaterDestroyTime);
    }

    void FreezeRay()
    {
        float maxFreezeDist = 100.0f;
        float minFreezeDist = 0.75f;
        // TODO: make it a "real" line, or some way for the player to see what's happening
        Debug.DrawLine(AimPos + (AimDirectionNorm * minFreezeDist), AimPos + (AimDirectionNorm*10.0f), Color.blue);

 


        // stop ray at wall and measure length
        var raycastHit = Physics2D.Raycast(AimPos, AimDirectionNorm, maxFreezeDist, FreezeRayStopLayers);
        if ( raycastHit )
        {
            maxFreezeDist = (AimPos - (Vector2)raycastHit.transform.position).magnitude;
            Debug.Log(maxFreezeDist);
        }


        // cast ray again, this time, hit the water layer
        var waterHits = Physics2D.RaycastAll(AimPos, AimDirectionNorm, maxFreezeDist, FreezeRayLayers);
 
        foreach ( RaycastHit2D hit in waterHits )
        {
            if(hit.distance < minFreezeDist) continue; // do not freeze inside player char

            var tempIce = Instantiate(IcePrefab, hit.transform.position, hit.transform.rotation);
            Destroy(hit.transform.gameObject);

            // TODO: maybe turn ice back to liquid after 5 secs?
            Destroy(tempIce, StaticConstants.IceDestroyTime);
        }
    }
}
