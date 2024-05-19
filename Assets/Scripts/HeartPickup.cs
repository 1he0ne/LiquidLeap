using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    private PlayerHealth PlayerHP;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerHP.Damage(-1);
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerHP = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }
}
