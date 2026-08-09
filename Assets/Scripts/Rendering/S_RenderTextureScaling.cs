using UnityEngine;
using UnityEngine.UI;

public class RenderTextureScaling : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;
    [SerializeField]
    private RawImage outputImage;

    [Header("Internal Resolution ")]
    [SerializeField]
    private int baseWidth = 1920, baseHeight = 1080;

    [Header("Performance Scaling")]
    [Range(0.5f, 1f)]
    [SerializeField]
    private float resolutionScale = 1f;

    [Header("Aspect Ratio")]
    [SerializeField]
    private float targetAspect = 16f / 9f;

    private RenderTexture currentRT;

    void Start()
    {
        ApplyResolution();
        ApplyAspectCorrection();
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        ReleaseRT();
    }

    private void ApplyResolution()
    {
        ReleaseRT();

        int width = Mathf.RoundToInt(baseWidth * resolutionScale);
        int height = Mathf.RoundToInt(baseHeight * resolutionScale);

        width = Mathf.Max(width, 320);
        height = Mathf.Max(height, 180);

        currentRT = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32)
        {
            useDynamicScale = false,
            filterMode = FilterMode.Bilinear, // Point for pixel art
            wrapMode = TextureWrapMode.Clamp,
            name = $"RT_{width}x{height}"
        };

        currentRT.Create();

        targetCamera.targetTexture = currentRT;
        outputImage.texture = currentRT;
    }

    private void ApplyAspectCorrection()
    {
        RectTransform rt = outputImage.rectTransform;

        float screenAspect = (float)Screen.width / Screen.height;

        if (screenAspect > targetAspect)
        {
            // Pillarbox
            float height = Screen.height;
            float width = height * targetAspect;
            rt.sizeDelta = new Vector2(width, height);
        }
        else
        {
            // Letterbox
            float width = Screen.width;
            float height = width / targetAspect;
            rt.sizeDelta = new Vector2(width, height);
        }

        rt.anchoredPosition = Vector2.zero;
    }

    private void ReleaseRT()
    {
        if (currentRT == null)
            return;

        if (targetCamera.targetTexture == currentRT)
            targetCamera.targetTexture = null;

        currentRT.Release();
        Destroy(currentRT);
        currentRT = null;
    }
}
