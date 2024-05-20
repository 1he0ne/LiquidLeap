using UnityEngine;


public class PersistentStorage : MonoBehaviour
{
    public static PersistentStorage Instance;
    public static AudioSource BGM;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (BGM == null)
        {
            BGM = gameObject.AddComponent<AudioSource>();
            BGM.volume = 0.3f;
            BGM.clip = Resources.Load<AudioClip>("Music/HealingWaves");
            BGM.loop = true;
            BGM.Play();
        }
    }


    public static int NumPlayerLives; // update this whenever loading / exiting a level

    private const int numberOfLevels = 3; // Tutorial, Level 1, Level 2
    public static bool[] LevelFinished = new bool[numberOfLevels];
    public static bool[] RubiesFound = new bool[numberOfLevels+1]; // there are 2 rubies in the last level...

    public static int NumRubiesFound()
    {
        int counter = 0;
        for (var i = 0; i < MaxRubies(); ++i)
        {
            if (RubiesFound[i]) ++counter;
        }

        return counter;
    }
    public static int MaxRubies() { return RubiesFound.Length; }

    public static int NumLevelsDone()
    {
        int counter = 0;
        for (var i = 0; i < MaxLevels(); ++i)
        {
            if (LevelFinished[i]) ++counter;
        }

        return counter;
    }
    public static int MaxLevels() { return LevelFinished.Length; }
}
