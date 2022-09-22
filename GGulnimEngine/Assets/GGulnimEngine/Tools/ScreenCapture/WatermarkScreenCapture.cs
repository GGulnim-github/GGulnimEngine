using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class WatermarkScreenCapture : MonoBehaviour
{
    public Texture2D WatermarkTexture = null;
    public TextureExtensions.Watermark_Anchor WatermarkAnchor = TextureExtensions.Watermark_Anchor.TopLeft;
    public float WatermarkScaleFactor = 1.0f;
    public int WatermarkOffsetX = 0;
    public int WatermarkOffsetY = 0;
    
    public Texture2D ScreenCaptureTexture = null;

    public int ScreenWidth = 0;
    public int ScreenHeight = 0;

    private bool ScreenCaptureWithUI_Ing = false;
    private bool ScreenCaptureWithoutUI_Ing = false;
    private bool UseWatermark = true;

    private string Day;
    public string FilePath;
    public string FolderName;
    public string FileName;

    public void Awake()
    {
        ScreenCaptureTexture = null;

        FilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        FolderName =  Application.productName;
    }

    public void ScreenCaptureWithUI(bool useWatermark)
    {
        if (ScreenCaptureWithUI_Ing)
            return;
        StartCoroutine(Co_ScreenCapture());
        UseWatermark = useWatermark;
    }

    private IEnumerator Co_ScreenCapture()
    {
        ScreenCaptureWithUI_Ing = true;
        yield return new WaitForEndOfFrame();
        ScreenCapture();
        ScreenCaptureWithUI_Ing = false;
    }

    public void ScreenCaptureWithoutUI(bool useWatermark)
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
        ScreenCaptureWithoutUI_Ing = true;
        UseWatermark = useWatermark;
    }
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        if (!ScreenCaptureWithoutUI_Ing)
            return;
        ScreenCapture();
        ScreenCaptureWithoutUI_Ing = false;
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }

    private void ScreenCapture()
    {
        SetFileName();

        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;

        ScreenCaptureTexture = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.RGB24, false);
        ScreenCaptureTexture.ReadPixels(new Rect(0, 0, ScreenWidth, ScreenHeight), 0, 0);

        if (UseWatermark)
        {
            ScreenCaptureTexture = ScreenCaptureTexture.AddWaterMark(WatermarkTexture, WatermarkAnchor, WatermarkScaleFactor, WatermarkOffsetX, WatermarkOffsetY);
        }
        else
        {
            ScreenCaptureTexture.Apply();
        }
    }

    private void SetFileName()
    {
        Day = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        FileName = $"GGulnimEngine_{Day}.png";
    }

    public void Save()
    {
        string directory = Path.Combine(FilePath, FolderName);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        byte[] byteArray = ScreenCaptureTexture.EncodeToPNG();
        string filePath = Path.Combine(directory, FileName);
        File.WriteAllBytes(filePath, byteArray);
    }
}
