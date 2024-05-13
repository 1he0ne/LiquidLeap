using System.Net.Sockets;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float Force;
    [SerializeField] public GameObject WaterPrefab;

    public Transform AimingPoint;

    public GameObject Gun;
    public SpriteRenderer GunSprite,PlayerSprite;

    public Vector3 WorldMousePos;
    public Vector2 Direction;
    public float shootTimer = 0f;
    public bool canShoot = true;
    public float shootDuration = 1f; // Time in seconds player can shoot continuously
    public float shootCooldown = 1f;

    void Update()
    {
       

        WorldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction = (Vector2)(WorldMousePos - transform.position).normalized;

        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 targetPosition = AimingPoint.position + (Vector3)(Direction * 0.5f);

        Gun.transform.position = targetPosition;

        Gun.transform.rotation = targetRotation;

        // TODO: flip gun, when rotated accordingly
        if (Mathf.Abs(angle) > 90)
        {
            GunSprite.flipY = true;
            PlayerSprite.flipX = true;
          
        }
        if ( Mathf.Abs(angle) < 90 )
        {
            GunSprite.flipY = false;
            PlayerSprite.flipX = false;
            
        }
        if ( canShoot )
        {
            // Check if the mouse button is pressed and the shoot duration hasn't elapsed
            if ( Input.GetMouseButton(0) && shootTimer <= shootDuration )
            {
                Shoot();
                shootTimer += Time.deltaTime;
            }
            else
            {
                canShoot = false;
                shootTimer = 0f;
            }
        }
        else
        {
            // Player can't shoot, start cooldown
            shootTimer += Time.deltaTime;
            if ( shootTimer >= shootCooldown )
            {
                canShoot = true;
                shootTimer = 0f;
            }
        }
    }

    void Shoot()
    {

        // Creates the bullet locally
        GameObject bullet = Instantiate(WaterPrefab,AimingPoint.position + (Vector3)(Direction * 0.5f),Quaternion.identity);  

        // Adds velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = Direction * Force;
        Destroy(bullet.gameObject, 5f);
    }
}
