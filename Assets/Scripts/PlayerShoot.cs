using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float Force;
    [SerializeField] public GameObject WaterPrefab;
    [SerializeField] public GameObject IcePrefab;

    public Transform AimingPoint;

    public GameObject Gun;
    public SpriteRenderer GunSprite,PlayerSprite;

    public Vector3 WorldMousePos;
    public Vector2 Direction;

    void Update()
    {
        WorldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction = (Vector2)(WorldMousePos - transform.position).normalized;

        Debug.DrawLine(WorldMousePos, transform.position);

        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 targetPosition = AimingPoint.position + (Vector3)(Direction * 0.5f);

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

        Debug.Log(angle);

        if ( Input.GetMouseButton(0) )
        {
            Shoot();
        }
    }

    void Shoot()
    {

        // Creates the water locally
        GameObject waterParticle = Instantiate(WaterPrefab, AimingPoint.position + (Vector3)(Direction * 0.5f), Quaternion.identity);  

        // Adds velocity to the bullet
        waterParticle.GetComponent<Rigidbody2D>().velocity = Direction * Force;
        Destroy(waterParticle.gameObject, 5f);


        // Creates the ice locally
        GameObject iceParticle = Instantiate(IcePrefab, AimingPoint.position - (Vector3)(Direction * 0.5f), Quaternion.identity);

        Destroy(iceParticle.gameObject, 5f);
    }   
}
