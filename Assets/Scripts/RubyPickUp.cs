using UnityEngine;

public class RubyPickUp : MonoBehaviour
{
    public int RubyID;

    private void Start()
    {
        if (PersistentStorage.RubiesFound[RubyID])
        {
            Destroy(gameObject);
        }
    }
}
