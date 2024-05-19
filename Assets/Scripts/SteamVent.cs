using UnityEngine;

public class SteamVent : MonoBehaviour
{
    public GameObject steamParticle; // TODO: should be loaded from Resources
    public GameObject waterParticle; // TODO: should be loaded from Resources

    private Transform steamEmitterTransform;
    private Transform waterEmitterTransform;

    public enum State { OFF, WATER, STEAM };

    public State state; // TODO: should be changed with a setter, not be public

    public Vector3 projectileStartSpeed = Vector3.zero;

    [SerializeField] private int emitterCooldownMax = 7;
    private int emitterCooldown;

    private GameObject Player;

    private void Start()
    {
        Player = GameObject.Find("Player");

        steamEmitterTransform = transform.Find("SteamEmitter");
        waterEmitterTransform = transform.Find("WaterEmitter");
    }

    private void FixedUpdate()
    {
        // do not produce, if there's no player, or if the player is far away
        if ( Player == null || Mathf.Abs(transform.position.x - Player.transform.position.x) > 15f ) return;

        if ( emitterCooldown <= 0 )
        {
            emitterCooldown = emitterCooldownMax;
            GameObject particle;
            Vector3 randOffset = new Vector2(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f)); 
            switch (state)
            {
                case State.WATER:
                    particle = Instantiate(waterParticle, waterEmitterTransform.position + randOffset, Quaternion.identity);
                    particle.GetComponent<Rigidbody2D>().velocity = projectileStartSpeed + randOffset*3.0f;
                    Destroy(particle, StaticConstants.SteamCondenseTime);
                    break;
                case State.STEAM:
                    particle = Instantiate(steamParticle, steamEmitterTransform.position + randOffset, Quaternion.identity);
                    particle.GetComponent<Rigidbody2D>().velocity = projectileStartSpeed + randOffset*3.0f;
                    Destroy(particle, StaticConstants.SteamCondenseTime);
                    break;
                case State.OFF:
                default:
                    break;
            }
        }
        emitterCooldown--;
    }

}
