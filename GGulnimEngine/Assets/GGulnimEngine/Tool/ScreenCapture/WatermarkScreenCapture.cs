using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class WatermarkScreenCapture : MonoBehaviour
{
    [Header("Watermark")]
    public Texture2D WatermarkTexture = null;
    public TextureExtension.Watermark_Anchor WatermarkAnchor = TextureExtension.Watermark_Anchor.TopLeft;
    public float WatermarkScaleFactor = 1.0f;
    public int WatermarkOffsetX = 0;
    public int WatermarkOffsetY = 0;
    
    [Space(10)]
    [Header("Result ScreenCaptuer")]
    [HideInInspector] public Texture2D ScreenCaptureTexture = null;

    private int ScreenWidth = 0;
    private int ScreenHeight = 0;

    private bool ScreenCaptureWithUI_Ing = false;
    private bool ScreenCaptureWithoutUI_Ing = false;

    private void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }
    private void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }

    public void Awake()
    {
        ScreenCaptureTexture = null;
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        if (!ScreenCaptureWithoutUI_Ing)
            return;

        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;

        ScreenCaptureTexture = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.RGB24, false);
        ScreenCaptureTexture.ReadPixels(new Rect(0, 0, ScreenWidth, ScreenHeight), 0, 0);

        ScreenCaptureTexture = ScreenCaptureTexture.AddWaterMark(WatermarkTexture,  WatermarkAnchor, WatermarkScaleFactor, WatermarkOffsetX, WatermarkOffsetY);

        ScreenCaptureWithoutUI_Ing = false;
    }

    public void DoScreenCaptureWithUI()
    {
        if (ScreenCaptureWithUI_Ing)
            return;
        StartCoroutine(ScreenCapture());
    }

    public void DoScreenCaptureWithoutUI()
    {
        ScreenCaptureWithoutUI_Ing = true;
    }

    private IEnumerator ScreenCapture()
    {
        ScreenCaptureWithUI_Ing = true;   
        yield return new WaitForEndOfFrame();

        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;

        ScreenCaptureTexture = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.RGB24, false);
        ScreenCaptureTexture.ReadPixels(new Rect(0, 0, ScreenWidth, ScreenHeight), 0, 0);

        ScreenCaptureTexture = ScreenCaptureTexture.AddWaterMark(WatermarkTexture, WatermarkAnchor, WatermarkScaleFactor, WatermarkOffsetX, WatermarkOffsetY);

        ScreenCaptureWithUI_Ing = false;
    }
}
