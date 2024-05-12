using System.Net.Sockets;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float Force;
    [SerializeField] public GameObject WaterPrefab;

    public Transform AimingPoint;

    void Update()
    {
        if( Input.GetMouseButton(0) )
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = (Vector2)((worldMousePos - transform.position)); direction.Normalize();

        // Creates the bullet locally
        GameObject bullet = Instantiate(WaterPrefab,AimingPoint.position + (Vector3)(direction * 0.5f),Quaternion.identity);

        // Adds velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = direction * Force;
    }
}
