using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
    public static PersistentStorage Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static int NumPlayerLives; // update this whenever loading / exiting a level

    private const int numberOfLevels = 3; // Tutorial, Level 1, Level 2
    public static bool[] LevelFinished = new bool[numberOfLevels];
    public static bool[] RubiesFound = new bool[numberOfLevels+1]; // there are 2 rubies in the last level...

}
