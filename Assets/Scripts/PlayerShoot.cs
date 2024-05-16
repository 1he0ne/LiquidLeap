using TMPro;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float Force;
    [SerializeField] private TextMeshProUGUI GunFillUI;

    public GameObject WaterPrefab;

    public Transform AimingPoint;

    public SpriteRenderer PlayerSprite;

    public GameObject Gun;
    private SpriteRenderer GunSprite;

    private Vector2 WorldMousePos;
    public static Vector2 AimPos; // not nice, but the ray caster needs to know these values
    public static Vector2 AimDirectionNorm; // not nice, but the ray caster needs to know these values


    public const float shootWaterTankMax = 36; // max tank fill state
    public float shootCooldownMax = 0.02f; // time between individual bullets

    private float shootWaterTank = shootWaterTankMax; // current fill state
    private float shootCooldown = 0f; // time until the next individual bullet

    private float waterFillPercent;

    private AudioSource WaterHoseSFX;
    private AudioSource WaterPumpSFX;


    private void Start()
    {
        GunSprite = Gun.GetComponent<SpriteRenderer>();
        var GunAudioSources = Gun.GetComponents<AudioSource>();

        WaterHoseSFX = GunAudioSources[0];
        WaterPumpSFX = GunAudioSources[1];
    }

    private void RechargeGun()
    {
        waterFillPercent = shootWaterTank / shootWaterTankMax;

        // recharge slowly if not at max
        if (shootWaterTank < shootWaterTankMax)
        {
            if (Input.GetMouseButton(0))
            {
                WaterHoseSFX.Stop();
                // if pressing fire, only let the gun trickle
                shootWaterTank = Mathf.Min(shootWaterTankMax, shootWaterTank + Time.deltaTime * 2.0f);

                if (!WaterPumpSFX.isPlaying && shootWaterTank > 1)
                {
                    WaterPumpSFX.Play();
                }
                else if (shootWaterTank <= 1)
                {
                    WaterPumpSFX.Stop();
                }
                WaterPumpSFX.volume = waterFillPercent;
            }
            else
            {
                WaterPumpSFX.Stop();
                // if not pressing fire, recharge the gun very quickly
                shootWaterTank = Mathf.Min(shootWaterTankMax, shootWaterTank + Time.deltaTime / (shootCooldownMax * 2.0f));

                if (!WaterHoseSFX.isPlaying)
                {
                    WaterHoseSFX.Play();
                }
            }
        }
        else
        {
            WaterHoseSFX.Stop();
        }

        // tick down the time for the next bullet to be shot
        shootCooldown -= Time.deltaTime;

        if (GunFillUI)
        {
            GunFillUI.text = string.Format("Fill status: {0:0}%", waterFillPercent * 100);
        }
    }


    void Update()
    {
        RechargeGun();

        WorldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        AimDirectionNorm = (WorldMousePos - (Vector2)transform.position).normalized;
        AimPos = AimingPoint.position;

        float angle = Mathf.Atan2(AimDirectionNorm.y, AimDirectionNorm.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector2 targetPosition = AimPos + (AimDirectionNorm * 0.5f);


        Gun.transform.position = targetPosition;
        Gun.transform.rotation = targetRotation;

        bool flipSprites = Mathf.Abs(angle) > 90;

        GunSprite.flipY = flipSprites;
        PlayerSprite.flipX = flipSprites;

        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // if gun is cooling down, or the tank is empty, don't shoot
        if (shootCooldown > 0 || shootWaterTank < 1) return;


        shootWaterTank--; // deplete the tank
        shootCooldown = shootCooldownMax; // pause briefly between water droplets (frame independent fire rate)

        // Creates the water locally
        GameObject waterParticle = Instantiate(WaterPrefab, AimPos + (AimDirectionNorm * 0.75f), Quaternion.identity);

        // Adds velocity to the bullet
        var waterVelocity = AimDirectionNorm * Force;

        // Reduce velocity as tank depletes
        if (waterFillPercent < 0.9f)
        {
            waterVelocity *= waterFillPercent;
        }

        waterParticle.GetComponent<Rigidbody2D>().velocity = waterVelocity;
        Destroy(waterParticle.gameObject, StaticConstants.WaterDestroyTime);
    }
}
