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
        //add force/velocity
        //Make it instantiate bullet where mouse position is currently
        //Damage enemy function
        //Destroy object after certain seconds  
        GameObject bullet = Instantiate(WaterPrefab, AimingPoint.position, AimingPoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = transform.right * Force;

        Destroy( bullet, 15f );
    }
}
