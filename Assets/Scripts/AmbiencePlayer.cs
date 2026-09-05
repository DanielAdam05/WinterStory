using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbiencePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool playOnAwake = true;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    private void Reset()
    {
        // Auto-fill the AudioSource reference when the component is first added
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        audioSource.loop = loop;
        audioSource.volume = volume;
    }

    private void Start()
    {
        if (playOnAwake)
        {
            audioSource.Play();
        }
    }
}
