using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float Force;
    [SerializeField] public GameObject WaterPrefab;
    [SerializeField] public GameObject IcePrefab;
    [SerializeField] public GameObject SteamPrefab;

    [SerializeField] public LayerMask RayStopLayers;
    [SerializeField] public LayerMask LiquidParticleLayers;


    public Transform AimingPoint;

    public SpriteRenderer PlayerSprite;

    public GameObject Gun;
    private SpriteRenderer GunSprite;

    private Vector2 WorldMousePos;
    private Vector2 AimPos;
    private Vector2 AimDirectionNorm;

    public float shootTimer = 0f;
    public bool canShoot = true;
    public float shootDuration = 1f; // Time in seconds player can shoot continuously
    public float shootCooldown = 1f;


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

        if (Mathf.Abs(angle) > 90)
        {
            GunSprite.flipY = true;
            PlayerSprite.flipX = true;
        }

        if (Mathf.Abs(angle) < 90)
        {
            GunSprite.flipY = false;
            PlayerSprite.flipX = false;
        }
        if (canShoot)
        {
            // Check if the mouse button is pressed and the shoot duration hasn't elapsed
            if (Input.GetMouseButton(0) && shootTimer <= shootDuration)
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
            if (shootTimer >= shootCooldown)
            {
                canShoot = true;
                shootTimer = 0f;
            }
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
    }

    void Shoot()
    {
        // Creates the water locally
        GameObject waterParticle = Instantiate(WaterPrefab, AimPos + (AimDirectionNorm * 0.5f), Quaternion.identity);

        // Adds velocity to the bullet
        waterParticle.GetComponent<Rigidbody2D>().velocity = AimDirectionNorm * Force;
        Destroy(waterParticle.gameObject, StaticConstants.WaterDestroyTime);
    }

    private List<RaycastHit2D> GetParticlesInRay(LayerMask rayStopLayers, LayerMask rayParticleLayers, Color rayColor)
    {
        float maxRayDist = 100.0f;
        float minRayDist = 0.75f;

        // stop ray at e.g. walls and determine the new max length up to that wall
        var raycastHit = Physics2D.Raycast(AimPos, AimDirectionNorm, maxRayDist, rayStopLayers);
        if (raycastHit)
        {
            maxRayDist = (AimPos - (Vector2)raycastHit.transform.position).magnitude;
        }


        // TODO: make it a "real" line, or some way for the player to see what's happening
        Debug.DrawLine(AimPos + (AimDirectionNorm * minRayDist), AimPos + (AimDirectionNorm * 10.0f), rayColor);

        // cast ray again, this time, hit the particle layer
        var waterHits = Physics2D.RaycastAll(AimPos, AimDirectionNorm, maxRayDist, rayParticleLayers);

        List<RaycastHit2D> particlesHit = new();

        foreach (RaycastHit2D hit in waterHits)
        {
            // skip particles that are very close
            if (hit.distance >= minRayDist)
            {
                particlesHit.Add(hit);
            }
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
