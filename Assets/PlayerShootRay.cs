using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerShootRay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RayStatsUI;

    public LayerMask RayStopLayers;
    public LayerMask LiquidParticleLayers;
    public LayerMask DeviceLayer;

    public GameObject IcePrefab;
    public GameObject SteamPrefab;


    public float fireRayMaxTime = 2.0f; // Maximum time the ray can fire before it needs to recharge
    public float rechargeRayTime = 5.5f; // Time required to recharge, should be longer than ice-unfreeze-time

    private bool isRayFiring = false;
    private bool isRayRecharging = false;
    private float fireRayStartTime;
    private float rechargeRayTimer = 0f;

    private LineRenderer beam;

    public Color iceColor = Color.blue;
    public Color fireColor = Color.red;


    // private LineRenderer RayGraphics;

    // Start is called before the first frame update
    void Start()
    {
        // RayGraphics = GetComponent<LineRenderer>();
        // RayGraphics.startWidth = 0.07f;
        // RayGraphics.endWidth = 0.05f;

        beam = this.gameObject.AddComponent<LineRenderer>();
        beam.startWidth = 0.05f;
        beam.endWidth = 0.05f;
        beam.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
    }

    void Update()
    {
        RechargeRay();
        CheckRayFire();

        float rayCooldownUIText = isRayFiring ? fireRayMaxTime - (Time.time - fireRayStartTime) : fireRayMaxTime;
        RayStatsUI.text = string.Format("Ray duration: {0:0.0}s\nRay cooldown: {1:0.0}s", isRayRecharging ? 0 : rayCooldownUIText, rechargeRayTimer);
    }


    private void RechargeRay()
    {
        if (isRayRecharging)
        {
            rechargeRayTimer -= Time.deltaTime;
            if (rechargeRayTimer <= 0f)
            {
                isRayRecharging = false;
                rechargeRayTimer = 0f;
            }
        }
    }

    private void CheckRayFire()
    {
        if ((Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1)) && !isRayRecharging)
        {
            StartFiringRay();
        }

        if (isRayFiring)
        {
            if (Input.GetMouseButtonUp(2) || Input.GetMouseButtonUp(1) || (Time.time - fireRayStartTime) >= fireRayMaxTime)
            {
                StopFiringRay();
            }
            else
            {
                if (Input.GetMouseButton(2))
                {
                    HeatRay();
                }
                else if (Input.GetMouseButton(1))
                {
                    FreezeRay();
                }
            }
        }
    }

    private void StartFiringRay()
    {
        beam.enabled = true;
        isRayFiring = true;
        fireRayStartTime = Time.time;
    }

    private void StopFiringRay()
    {
        beam.enabled = false;
        isRayFiring = false;
        isRayRecharging = true;
        rechargeRayTimer = rechargeRayTime;
    }



    private List<RaycastHit2D> GetParticlesInRay(LayerMask rayParticleLayers, Color rayColor)
    {
        Vector2 AimPos = PlayerShoot.AimPos;
        Vector2 AimDirectionNorm = PlayerShoot.AimDirectionNorm;

        float maxRayDist = 5.0f;
        float minRayDist = 0.75f;

        var rayStartPos = AimPos + (AimDirectionNorm * minRayDist);
        // stop ray at e.g. walls and determine the new max length up to that wall
        RaycastHit2D raycastHit = Physics2D.Raycast(rayStartPos, AimDirectionNorm, maxRayDist, RayStopLayers);
        if (raycastHit)
        {
            maxRayDist = raycastHit.distance;
            // Debug.Log(maxRayDist);
        }

        // TODO: make it a "real" line, or some way for the player to see what's happening
        //Debug.DrawLine(rayStartPos, rayStartPos + (AimDirectionNorm * maxRayDist), rayColor);
        //RayGraphics.SetPositions(new Vector3[]{ rayStartPos, rayStartPos + (AimDirectionNorm * maxRayDist)});
        beam.SetPosition(0, rayStartPos);
        beam.SetPosition(1, rayStartPos + (AimDirectionNorm * maxRayDist));

        Gradient tempGradient = new Gradient();
        GradientColorKey[] tempColorKeys = new GradientColorKey[2];
        tempColorKeys[0] = new GradientColorKey(rayColor, 0);
        tempColorKeys[1] = new GradientColorKey(rayColor, 1);
        tempGradient.colorKeys = tempColorKeys;

        beam.colorGradient = tempGradient;



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
        foreach (RaycastHit2D hit in GetParticlesInRay(LiquidParticleLayers, iceColor))
        {
            var tempIce = Instantiate(IcePrefab, hit.transform.position, hit.transform.rotation);
            Destroy(hit.transform.gameObject);

            Destroy(tempIce, StaticConstants.IceMeltTime);
        }

        foreach (RaycastHit2D hit in GetParticlesInRay(DeviceLayer, iceColor))
        {
            hit.transform.gameObject.GetComponent<SteamVent>().state = SteamVent.State.WATER;
        }
    }

    void HeatRay()
    {
        foreach (RaycastHit2D hit in GetParticlesInRay(LiquidParticleLayers, fireColor))
        {
            var tempSteam = Instantiate(SteamPrefab, hit.transform.position, hit.transform.rotation);
            Destroy(hit.transform.gameObject);

            tempSteam.GetComponent<RevertToWater>().turnToWater = true;
            Destroy(tempSteam, StaticConstants.SteamCondenseTime);
        }

        foreach (RaycastHit2D hit in GetParticlesInRay(DeviceLayer, fireColor))
        {
            hit.transform.gameObject.GetComponent<SteamVent>().state = SteamVent.State.STEAM;
        }
    }
}
