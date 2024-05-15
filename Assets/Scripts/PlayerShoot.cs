using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float Force;
    [SerializeField] private TextMeshProUGUI GunFillUI;

    public GameObject WaterPrefab;
    public GameObject IcePrefab;
    public GameObject SteamPrefab;

    public LayerMask RayStopLayers;
    public LayerMask LiquidParticleLayers;


    public Transform AimingPoint;

    public SpriteRenderer PlayerSprite;

    public GameObject Gun;
    private SpriteRenderer GunSprite;

    private Vector2 WorldMousePos;
    private Vector2 AimPos;
    private Vector2 AimDirectionNorm;


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
                shootWaterTank = Mathf.Min(shootWaterTankMax, shootWaterTank + Time.deltaTime*2.0f);

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
                shootWaterTank = Mathf.Min(shootWaterTankMax, shootWaterTank + Time.deltaTime / (shootCooldownMax*2.0f));

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

        GunFillUI.text = string.Format("Fill status: {0:0}%", waterFillPercent*100);
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

        if (Input.GetMouseButton(1))
        {
            FreezeRay();
        }

        if (Input.GetMouseButton(2))
        {
            HeatRay();
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

    private List<RaycastHit2D> GetParticlesInRay(LayerMask rayStopLayers, LayerMask rayParticleLayers, Color rayColor)
    {
        float maxRayDist = 5.0f;
        float minRayDist = 0.75f;

        var rayStartPos = AimPos + (AimDirectionNorm * minRayDist);

        // stop ray at e.g. walls and determine the new max length up to that wall
        RaycastHit2D raycastHit = Physics2D.Raycast(rayStartPos, AimDirectionNorm, maxRayDist, rayStopLayers);
        if (raycastHit)
        {
            maxRayDist = raycastHit.distance;
            // Debug.Log(maxRayDist);
        }

        // TODO: make it a "real" line, or some way for the player to see what's happening
        Debug.DrawLine(rayStartPos, rayStartPos + (AimDirectionNorm * maxRayDist), rayColor);


        // cast ray again, this time, hit the particle layer
        var waterHits = Physics2D.RaycastAll(rayStartPos, AimDirectionNorm, maxRayDist, rayParticleLayers);

        List<RaycastHit2D> particlesHit = new();

        foreach (RaycastHit2D hit in waterHits)
        {
            particlesHit.Add(hit);
        }

        return particlesHit;
    }
    void FreezeRay()
    {
        foreach (RaycastHit2D hit in GetParticlesInRay(RayStopLayers, LiquidParticleLayers, Color.blue))
        {
            var tempIce = Instantiate(IcePrefab, hit.transform.position, hit.transform.rotation);
            Destroy(hit.transform.gameObject);

            Destroy(tempIce, StaticConstants.IceMeltTime);
        }
    }

    void HeatRay()
    {
        foreach (RaycastHit2D hit in GetParticlesInRay(RayStopLayers, LiquidParticleLayers, Color.red))
        {
            var tempSteam = Instantiate(SteamPrefab, hit.transform.position, hit.transform.rotation);
            Destroy(hit.transform.gameObject);

            Destroy(tempSteam, StaticConstants.SteamCondenseTime);
        }
    }
}
