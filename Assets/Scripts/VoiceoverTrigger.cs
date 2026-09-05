using UnityEngine;

public class VoiceoverTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool playOnlyOnce = true;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (playOnlyOnce && hasPlayed) return;

        VoiceoverManager.Instance.RequestPlay(audioSource);
        hasPlayed = true;
    }
}
