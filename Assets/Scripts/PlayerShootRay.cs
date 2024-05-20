using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerShootRay : MonoBehaviour
{
    public GameObject IcePrefab; // TODO: should be loaded from Resources
    public GameObject SteamPrefab; // TODO: should be loaded from Resources

    [SerializeField] private LayerMask RayStopLayers;
    [SerializeField] private LayerMask LiquidParticleLayers;
    [SerializeField] private LayerMask SteamParticleLayers;
    [SerializeField] private LayerMask IceParticleLayers;
    [SerializeField] private LayerMask DeviceLayer;

    [SerializeField] private Color iceColor = Color.blue;
    [SerializeField] private Color heatColor = Color.red;

    [SerializeField] private float fireRayMaxTime = 1.0f; // Maximum time the ray can fire before it needs to recharge
    private float rechargeRayTime = 4.0f; // Time required to recharge, should be longer than ice-unfreeze-time

    [SerializeField] private AudioSource freezeRaySFX;
    [SerializeField] private AudioSource heatRaySFX;

    private bool isRayFiring = false;
    private bool isRayRecharging = false;
    private float fireRayStartTime;
    private float rechargeRayTimer = 0f;

    private LineRenderer beam;
    private TextMeshProUGUI RayStatsUI;

    void Start()
    {
        beam = gameObject.AddComponent<LineRenderer>();
        beam.startWidth = 0.05f;
        beam.endWidth = 0.05f;
        beam.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

        RayStatsUI = GameObject.Find("RayStatsUI").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        RechargeRay();
        CheckRayFire();


        // if (RayStatsUI)
        // {
        //     RayStatsUI.text = GetRayStatsText();
        // }
    }

    public string GetRayStatsText()
    {
        float rayCooldownUIText = isRayFiring ? fireRayMaxTime - (Time.time - fireRayStartTime) : fireRayMaxTime;
        return string.Format("Ray duration: {0:0.0}s\nRay cooldown: {1:0.0}s", isRayRecharging ? 0 : rayCooldownUIText, rechargeRayTimer);
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
        if ((Input.GetButtonDown("Fire3") || Input.GetButtonDown("Fire2")) && !isRayRecharging)
        {
            StartFiringRay();
        }

        if (isRayFiring)
        {
            if (Input.GetButtonUp("Fire3") || Input.GetButtonUp("Fire2") || (Time.time - fireRayStartTime) >= fireRayMaxTime)
            {
                StopFiringRay();
                freezeRaySFX.Stop();
                heatRaySFX.Stop();
            }
            else
            {
                if (Input.GetButton("Fire3"))
                {
                    HeatRay();
                    if(!heatRaySFX.isPlaying)
                    {
                        heatRaySFX.Play();
                    }
                }
                else if (Input.GetButton("Fire2"))
                {
                    FreezeRay();
                    if (!freezeRaySFX.isPlaying)
                    {
                        freezeRaySFX.Play();
                    }
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
        foreach (RaycastHit2D hit in GetParticlesInRay(LiquidParticleLayers | SteamParticleLayers, iceColor))
        {
            var tempIce = Instantiate(IcePrefab, hit.transform.position, hit.transform.rotation);
            tempIce.transform.position = new Vector2(hit.transform.position.x + Random.Range(-0.05f, 0.05f), hit.transform.position.y);
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
        foreach (RaycastHit2D hit in GetParticlesInRay(LiquidParticleLayers | IceParticleLayers, heatColor))
        {
            var tempSteam = Instantiate(SteamPrefab, hit.transform.position, hit.transform.rotation);
            tempSteam.transform.position = new Vector2(hit.transform.position.x + Random.Range(-0.05f, 0.05f), hit.transform.position.y);
            Destroy(hit.transform.gameObject);

            tempSteam.GetComponent<RevertToWater>().turnToWater = true;
            Destroy(tempSteam, StaticConstants.SteamCondenseTime);
        }


        foreach (RaycastHit2D hit in GetParticlesInRay(DeviceLayer, heatColor))
        {
            hit.transform.gameObject.GetComponent<SteamVent>().state = SteamVent.State.STEAM;
        }
    }
}
