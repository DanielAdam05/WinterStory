using UnityEngine;

public class VoiceoverTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool playOnlyOnce = true;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger: " + other.gameObject.name + " | Tag: " + other.tag);
        if (!other.CompareTag("Player")) return;
        if (playOnlyOnce && hasPlayed) return;

        audioSource.Play();
        hasPlayed = true;
    }

  
}