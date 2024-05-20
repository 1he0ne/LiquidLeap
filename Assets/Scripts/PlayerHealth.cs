using UnityEngine;
using System.Linq;

public class PlayerHealth : MonoBehaviour, IDamageable
{ 
    public int Health { get; set; }

    private AudioSource Audio;
    private Animator Animator;
    private ParticleSystem Particle;
    private AudioClip HurtSFX;
    private AudioClip PickupSFX;

    private GameObject[] Hearts;
    private GameEndScript GameEnd;


    void Start()
    {
        Health = Mathf.Max(PersistentStorage.NumPlayerLives, 1);

        Audio = gameObject.AddComponent<AudioSource>();
        Animator = GetComponent<Animator>();
        Particle = Resources.Load<ParticleSystem>("PlayerDeathParticles");
        HurtSFX = Resources.Load<AudioClip>("SFX/PlayerHurt");
        PickupSFX = Resources.Load<AudioClip>("SFX/pickup");

        Hearts = GameObject.FindGameObjectsWithTag("UIHeart");
        Hearts = Hearts.OrderBy(heart => heart.transform.position.y)  // Sort by y position (top to bottom)
                                 .ThenBy(heart => -heart.transform.position.x) // Then by x position (left to right)
                                 .ToArray();
        UpdateHealthBarHeartCount(Health);

        GameEnd = GameObject.Find("GameEnd").GetComponent<GameEndScript>();
    }
    public void Damage(int value)
    {
        Health -= value;
        UpdateHealthBarHeartCount(Health);
    }

    // Update is called once per frame
    void Update()
    {
        if ( Health <= 0 )
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Enemy" )
        {
            Damage(1);
            Audio.PlayOneShot(HurtSFX, 0.5f);
            Animator.SetTrigger("PlayerHurt");
            Debug.Log("Ouch!");
        }
        
        if ( collision.collider.gameObject.tag == "Death" )
        {
            Damage(9999);
        }

        if ( collision.collider.gameObject.tag == "ExtraLife" )
        {
            Destroy(collision.collider.gameObject);
            Audio.PlayOneShot(PickupSFX, 0.5f);
            Damage(-1);
        }

        if ( collision.collider.gameObject.tag == "Ruby" )
        {
            var pickup = collision.collider.gameObject.GetComponent<RubyPickUp>();
            PersistentStorage.RubiesFound[pickup.RubyID] = true;
            

            Destroy(collision.collider.gameObject);
            Audio.PlayOneShot(PickupSFX, 0.5f);
            Damage(-2);
        }
    }

    public void Die()
    {
        switch(GameEnd.LevelId)
        {
            case 0:
                PersistentStorage.RubiesFound[0] = false;
                break;
            case 1:
                PersistentStorage.RubiesFound[1] = false;
                break;
            case 2:
                PersistentStorage.RubiesFound[2] = false;
                PersistentStorage.RubiesFound[3] = false;
                break;
        }

        Instantiate(Particle, transform.position, Quaternion.identity);
        Destroy(gameObject);

        //Destroy
        Debug.Log("Player Died");
    }

    public void UpdateHealthBarHeartCount(int health)
    {
        // make sure we stay within the index
        health = Mathf.Clamp(health, 0, Hearts.Length);

        for (var i = 0; i < Hearts.Length; ++i)
        {
            Hearts[i].SetActive(true);
        }

        // start in reverse
        for (var i = 0; i < Hearts.Length - health; ++i)
        {
            Hearts[i].SetActive(false);
        }
    }
}
