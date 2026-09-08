using System.Runtime.CompilerServices;
using UnityEngine;

public class LightingController : MonoBehaviour
{
    [SerializeField]
    private Light moonLight;

    [Header("Lighting Variables")]
    [SerializeField] private float startingMoonIntensity = 0.7f;
    [SerializeField] private float finalMoonIntensity = 0f;
    [Space(10)]
    [SerializeField] private float startingEnvLightIntensity = 1f;
    [SerializeField] private float finalEnvLightIntensity = 0f;
    [Space(3)]
    [SerializeField] private float startingEnvReflectionIntensity = 1f;
    [SerializeField] private float finalEnvReflectionIntensity = 0.1f;
    [Space(3)]
    [SerializeField] private float startingFogDensity = 0f;
    [SerializeField] private float finalFogDensity = 0.15f;

    // Private variables
    private float moonIntensityDelta;
    private float envLightIntensityDelta;
    private float envReflectionIntensityDelta;
    private float fogDensityDelta;

    private void Awake()
    {
        RestoreLighting();

        moonIntensityDelta = startingMoonIntensity - finalMoonIntensity;
        envLightIntensityDelta = startingEnvLightIntensity - finalEnvLightIntensity;
        envReflectionIntensityDelta = startingEnvReflectionIntensity - finalEnvReflectionIntensity;
        fogDensityDelta = startingFogDensity - finalFogDensity;
    }

    private void OnEnable()
    {
        VoiceSequenceManager.OnAnyVoiceStarted += HandleVoiceoverStarted;
        VoiceSequenceManager.OnAnyVoiceFinished += HandleVoiceoverFinished;
    }

    private void OnDisable()
    {
        VoiceSequenceManager.OnAnyVoiceStarted -= HandleVoiceoverStarted;
        VoiceSequenceManager.OnAnyVoiceFinished -= HandleVoiceoverFinished;
    }

    private void HandleVoiceoverStarted(int index)
    {
        DimLighting();
    }

    private void HandleVoiceoverFinished(int index)
    {
        //DimLighting();
    }

    private void DimLighting()
    {
        moonLight.intensity -= moonIntensityDelta / VoiceSequenceManager.Instance.VoiceoverCount;
        RenderSettings.ambientIntensity -= envLightIntensityDelta / VoiceSequenceManager.Instance.VoiceoverCount;
        RenderSettings.reflectionIntensity -= envReflectionIntensityDelta / VoiceSequenceManager.Instance.VoiceoverCount;
        RenderSettings.fogDensity -= fogDensityDelta / VoiceSequenceManager.Instance.VoiceoverCount;
    }

    private void RestoreLighting()
    {
        moonLight.intensity = startingMoonIntensity;
        RenderSettings.ambientIntensity = startingEnvLightIntensity;
        RenderSettings.reflectionIntensity = startingEnvReflectionIntensity;
        RenderSettings.fogDensity = startingFogDensity;
    }
}
