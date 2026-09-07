using UnityEngine;

public class VoiceSequenceManager : MonoBehaviour
{
    public static VoiceSequenceManager Instance { get; private set; }

    [SerializeField]
    private int nextIndexToPlay = 0;

    private bool isPlaying = false;

    private void Awake()
    {
        // Only one game object with the Singleton class can exist
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool CanPlay(int index)
    {
        return !isPlaying && index == nextIndexToPlay;
    }

    public void NotifyStarted()
    {
        isPlaying = true;
    }

    public void NotifyFinished()
    {
        isPlaying = false;
        ++nextIndexToPlay;
    }
}
