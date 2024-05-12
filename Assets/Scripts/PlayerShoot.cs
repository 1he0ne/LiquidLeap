using System.Net.Sockets;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float Force;
    [SerializeField] public GameObject WaterPrefab;

    public Transform AimingPoint;

    public GameObject Gun;
    public SpriteRenderer GunSprite;

    public Vector3 WorldMousePos;
    public Vector2 Direction;

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
            GunSprite.flipX = true;  
        }

        if ( Input.GetMouseButton(0) )
        {
            Shoot();
        }
    }

    void Shoot()
    {

        // Creates the bullet locally
        GameObject bullet = Instantiate(WaterPrefab,AimingPoint.position + (Vector3)(Direction * 0.5f),Quaternion.identity);  

        // Adds velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = Direction * Force;
    }
}
