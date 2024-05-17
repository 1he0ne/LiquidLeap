using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // isGrounded and isMoving are public, because the AnimationController uses them (not ideal design, but it's a game jam!)
    public bool isGrounded = false;
    public bool isMoving = false;

    [SerializeField] private SpriteRenderer PlayerRenderer;

    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask GroundLayers;

    private Rigidbody2D Rb;

    private SpriteRenderer ParachuteRendererL;
    private SpriteRenderer ParachuteRendererR;

    private Transform GroundCheck1;
    private Transform GroundCheck2;

    private bool isParachuteActive = false;

    private float MoveDir;

    private AudioSource JumpSound;
    private AudioSource WalkSound;
    private AudioSource LandFloorSound;
    private AudioSource WingFlapSound;
    private AudioSource SteamBoostSound;

    private const float parachuteCooldownMax = 1.0f;
    private float parachuteCooldown = 0.0f;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        ParachuteRendererL = transform.Find("WingsAnimSpriteL").GetComponent<SpriteRenderer>();
        ParachuteRendererR = transform.Find("WingsAnimSpriteR").GetComponent<SpriteRenderer>();

        GroundCheck1 = transform.Find("GroundCheck1").GetComponent<Transform>();
        GroundCheck2 = transform.Find("GroundCheck2").GetComponent<Transform>();

        if ( GroundCheck1 == null || GroundCheck2 == null )
        {
            Debug.LogError("GroundCheck locations not found. Please add 'GroundCheck1' and 'GroundCheck2' transforms!");
        }

        if( ParachuteRendererL == null || ParachuteRendererR == null ) 
        {
            Debug.LogError("Wings not found. Player needs WingsAnimSpriteL / WingsAnimSpriteR to function.");
        }

        var AudioSources = GetComponents<AudioSource>();
        JumpSound = AudioSources[0];
        WalkSound = AudioSources[1];
        LandFloorSound = AudioSources[2];
        WingFlapSound = AudioSources[3];
        SteamBoostSound = AudioSources[4];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isParachuteActive && collision.gameObject.layer == 10) // steam is layer 10
        {
            // oof make sure we only ever trigger on steam!
            Destroy(collision.gameObject);
            Rb.AddForce(new Vector2(0, JumpForce / 5.0f), ForceMode2D.Impulse);
            if (!SteamBoostSound.isPlaying)
            {
                SteamBoostSound.pitch = Random.Range(0.98f, 1.02f);
                SteamBoostSound.volume = Random.Range(0.3f, 0.5f);
                SteamBoostSound.Play();
            }
        }
    }


    void Update()
    {
        isMoving = false;

        MoveDir = Input.GetAxis("Horizontal");
        Rb.velocity = new Vector2(MoveDir * MovementSpeed, Rb.velocity.y);

        if (Rb.velocity.x > 0.1f)
        {
            isMoving = true;
            PlayerRenderer.flipX = false;
        }
        else if (Rb.velocity.x < -0.1f)
        {
            isMoving = true;
            PlayerRenderer.flipX = true;
        }


        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.Raycast(GroundCheck1.position, -transform.up, 0.15f, GroundLayers) || Physics2D.Raycast(GroundCheck2.position, -transform.up, 0.15f, GroundLayers);
        if (!wasGrounded && isGrounded)
        {
            LandFloorSound.pitch = Random.Range(0.95f, 1.05f);
            LandFloorSound.Play();
        }

        bool wasParachuteActive = parachuteCooldown <= 0;

        if (Input.GetButton("Jump"))
        {
            if (isGrounded)
            {
                parachuteCooldown = parachuteCooldownMax;
                if (Rb.velocity.y <= 0.01)
                {
                    Rb.velocity = new Vector2(Rb.velocity.x, JumpForce);

                    JumpSound.pitch = Random.Range(0.85f, 0.9f);
                    JumpSound.Play();
                }

                isMoving = false;
            }
            else
            {
                parachuteCooldown -= Time.deltaTime;
            }
        }
        else
        {
            parachuteCooldown = parachuteCooldownMax;

        }

        isParachuteActive = !isGrounded && parachuteCooldown <= 0;
        ParachuteRendererL.enabled = ParachuteRendererR.enabled = isParachuteActive;


        Rb.gravityScale = isParachuteActive ? 0.5f : 1.0f;

        if (!wasParachuteActive && isParachuteActive)
        {
            WalkSound.pitch = Random.Range(0.95f, 1.05f);
            WingFlapSound.Play();
            Rb.velocity = new Vector2(Rb.velocity.x, JumpForce / 5.0f); // add a small upward boost
        }


        if (!isMoving || !isGrounded)
        {
            WalkSound.Stop();
        }
        else if (!WalkSound.isPlaying && isGrounded && isMoving)
        {
            WalkSound.pitch = Random.Range(0.95f, 1.05f);
            WalkSound.Play();
        }
    }
}
