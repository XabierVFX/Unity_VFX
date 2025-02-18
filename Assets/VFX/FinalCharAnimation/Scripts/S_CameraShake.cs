using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Playables;
using UnityEditor;

public class S_CameraShake : MonoBehaviour
{
    // Cinemachine Camera Shake
    [Header("Cinemachine Camera Settings")]
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    // Shake Parameters
    [Header("Shake Settings")]
    public float shakeIntensity = 1.0f;
    public float shakeFrequency = 2.0f;
    public float shakeDuration = 1.0f;
    public AnimationCurve shakeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    // Post-Processing Effects
    [Header("Post-Processing Effects")]
    public Volume postProcessingVolume;
    private DepthOfField depthOfField;
    private ChromaticAberration chromaticAberration;
    private MotionBlur motionBlur;

    // Shake Visual Effects
    [Header("Shake Visual Effects")]
    public float shakeBlur = 2.0f;
    public float shakeDepthOfField = 10.0f;
    public float shakeChromaticAberration = 1.0f;
    public float shakeMotionBlur = 1.0f;

    private float initialBlur;
    private float initialDepthOfField;
    private float initialChromaticAberration;
    private float initialMotionBlur;

    private Coroutine shakeCoroutine;

    void OnEnable()
    {
        InitializeNoise();
    }

    void Start()
    {
        InitializeNoise();
    }

    // Initialize noise component and post-processing effects
    private void InitializeNoise()
    {
        if (virtualCamera == null)
            return;

        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise == null)
            noise = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise != null)
        {
            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;
        }

        if (postProcessingVolume != null)
        {
            postProcessingVolume.profile.TryGet(out depthOfField);
            postProcessingVolume.profile.TryGet(out chromaticAberration);
            postProcessingVolume.profile.TryGet(out motionBlur);

            if (depthOfField != null)
            {
                initialBlur = depthOfField.gaussianStart.value;
                initialDepthOfField = depthOfField.gaussianEnd.value;
            }

            if (chromaticAberration != null)
                initialChromaticAberration = chromaticAberration.intensity.value;

            if (motionBlur != null)
                initialMotionBlur = motionBlur.intensity.value;
        }
    }

    // Start the camera shake effect
    public void StartShake()
    {
        if (noise == null)
            return;

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        noise.m_AmplitudeGain = shakeIntensity;
        noise.m_FrequencyGain = shakeFrequency;

        shakeCoroutine = StartCoroutine(ShakeCoroutine());

#if UNITY_EDITOR
        EditorApplication.update += UpdateEditor;
        SceneView.RepaintAll();
#endif
    }

    // Coroutine to handle shake duration and intensity
    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0;

        while (elapsedTime < shakeDuration)
        {
            float shakeFactor = shakeCurve.Evaluate(elapsedTime / shakeDuration);

            if (noise != null)
            {
                noise.m_AmplitudeGain = shakeIntensity * shakeFactor;
                noise.m_FrequencyGain = shakeFrequency * shakeFactor;
            }

            if (depthOfField != null)
            {
                depthOfField.gaussianStart.value = Mathf.Lerp(initialBlur, shakeBlur, shakeFactor);
                depthOfField.gaussianEnd.value = Mathf.Lerp(initialDepthOfField, shakeDepthOfField, shakeFactor);
            }

            if (chromaticAberration != null)
                chromaticAberration.intensity.value = Mathf.Lerp(initialChromaticAberration, shakeChromaticAberration, shakeFactor);

            if (motionBlur != null)
                motionBlur.intensity.value = Mathf.Lerp(initialMotionBlur, shakeMotionBlur, shakeFactor);

#if UNITY_EDITOR
            SceneView.RepaintAll();
#endif

            elapsedTime += Application.isPlaying ? Time.deltaTime : 0.02f;
            yield return null;
        }

        if (noise != null)
        {
            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;
        }

        if (depthOfField != null)
        {
            depthOfField.gaussianStart.value = initialBlur;
            depthOfField.gaussianEnd.value = initialDepthOfField;
        }

        if (chromaticAberration != null)
            chromaticAberration.intensity.value = initialChromaticAberration;

        if (motionBlur != null)
            motionBlur.intensity.value = initialMotionBlur;

#if UNITY_EDITOR
        EditorApplication.update -= UpdateEditor;
        SceneView.RepaintAll();
#endif
    }

#if UNITY_EDITOR
    // Update the editor scene view for real-time visualization
    private void UpdateEditor()
    {
        SceneView.RepaintAll();
    }
#endif
}
