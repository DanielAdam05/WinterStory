using System;
using UnityEngine;

public class VoiceSequenceManager : MonoBehaviour
{
    public static VoiceSequenceManager Instance { get; private set; }

    [SerializeField]
    private int nextIndexToPlay = 0;

    [SerializeField]
    private int voiceOverCount;

    private bool isPlaying = false;

    public static event Action<int> OnAnyVoiceStarted;
    public static event Action<int> OnAnyVoiceFinished;

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

    public void NotifyStarted(int index)
    {
        isPlaying = true;
        OnAnyVoiceStarted?.Invoke(index);
    }

    public void NotifyFinished(int index)
    {
        isPlaying = false;
        ++nextIndexToPlay;
        OnAnyVoiceFinished?.Invoke(index);
    }

    public int VoiceoverCount => voiceOverCount;
}
