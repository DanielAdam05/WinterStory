using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceoverManager : MonoBehaviour
{
    public static VoiceoverManager Instance { get; private set; }

    private readonly Queue<AudioSource> queue = new Queue<AudioSource>();
    private bool isProcessing = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RequestPlay(AudioSource source)
    {
        if (source == null) return;
        if (queue.Contains(source)) return; // avoid double-queueing the same clip

        queue.Enqueue(source);

        if (!isProcessing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isProcessing = true;

        while (queue.Count > 0)
        {
            AudioSource src = queue.Dequeue();
            src.Play();

            // Wait a frame so isPlaying has time to become true before we check it
            yield return null;
            yield return new WaitUntil(() => !src.isPlaying);
        }

        isProcessing = false;
    }
}
