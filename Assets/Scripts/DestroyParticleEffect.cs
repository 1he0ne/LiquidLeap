using UnityEngine;

public class DestroyParticleEffect : MonoBehaviour
{
    void Update()
    {
        Destroy(gameObject, 2f);
    }
}
