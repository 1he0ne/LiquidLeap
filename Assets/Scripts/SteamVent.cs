using UnityEngine;

public class SteamVent : MonoBehaviour
{
    public GameObject steamParticle;
    public GameObject waterParticle;

    public enum State { OFF, WATER, STEAM };

    public State state;

    private const int emitterCooldownMax = 5;
    private int emitterCooldown;

    private void FixedUpdate()
    {
        if (emitterCooldown <= 0)
        {
            emitterCooldown = emitterCooldownMax;
            GameObject particle;
            Vector3 randOffset = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)); 
            switch (state)
            {
                case State.WATER:
                    particle = Instantiate(waterParticle, transform.position + randOffset, Quaternion.identity);
                    Destroy(particle, StaticConstants.WaterDestroyTime);
                    break;
                case State.STEAM:
                    particle = Instantiate(steamParticle, transform.position + randOffset, Quaternion.identity);
                    Destroy(particle, StaticConstants.WaterDestroyTime);
                    break;
                case State.OFF:
                default:
                    break;
            }
        }
        emitterCooldown--;
    }

}
