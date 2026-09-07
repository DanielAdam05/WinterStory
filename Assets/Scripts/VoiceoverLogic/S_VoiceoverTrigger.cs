using System.Collections;
using UnityEditor;
using UnityEngine;

public class VoiceoverTrigger : MonoBehaviour
{
    public int sequenceIndex;

    public string playerTag = "Player";

    private AudioSource audioSource;
    [SerializeField]
    private bool hasPlayed = false; // Prevent re-triggering
    [SerializeField]
    private bool waitingForTurn = false; // player inside but not this index's turn yet

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed || !other.CompareTag(playerTag))
            return;

        // Check if can play
        if (VoiceSequenceManager.Instance.CanPlay(sequenceIndex))
        {
            PlayVoiceover();
        }
        else
        {
            waitingForTurn = true;
        }
    }

    private void Update()
    {
        if (!waitingForTurn || hasPlayed)
            return;

        if(VoiceSequenceManager.Instance.CanPlay(sequenceIndex))
        {
            PlayVoiceover();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if(other.CompareTag(playerTag))
        //    waitingForTurn = false;
    }

    private void PlayVoiceover()
    {
        Debug.Log($"FROM GO {gameObject.name}, Playing clip: {(audioSource.clip != null ? audioSource.clip.name : "NULL")}, isPlaying after Play(): {audioSource.isPlaying}");

        hasPlayed = true;
        waitingForTurn = false;

        VoiceSequenceManager.Instance.NotifyStarted();
        audioSource.Play();

        StartCoroutine(WaitUntilFinished());
    }

    private IEnumerator WaitUntilFinished()
    {
        yield return null;
        
        // Retutn null while playing
        while(audioSource.isPlaying)
        {
            yield return null;
        }

        // Notify finished when audioSource.isPlaying = false
        VoiceSequenceManager.Instance.NotifyFinished();
    }
}
