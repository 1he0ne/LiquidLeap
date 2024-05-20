using UnityEngine;

public class RubyPickUp : MonoBehaviour
{
    public int RubyID;

    private void OnDestroy()
    {
        PersistentStorage.RubiesFound[RubyID] = true;
    }
}
