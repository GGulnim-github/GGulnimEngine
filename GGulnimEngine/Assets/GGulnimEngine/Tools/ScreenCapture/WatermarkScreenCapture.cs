using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

[AddComponentMenu("GGulnim Engine/Tools/Watermark ScreenCapture")]
public class WatermarkScreenCapture : MonoBehaviour
{
    public Texture2D WatermarkTexture = null;
    public TextureExtensions.Watermark_Anchor WatermarkAnchor = TextureExtensions.Watermark_Anchor.TopLeft;
    public bool UseWatermark = true;
    public bool WithUI = false;
    public float WatermarkScaleFactor = 1.0f;
    public int WatermarkOffsetX = 0;
    public int WatermarkOffsetY = 0;
    
    public Texture2D ScreenCaptureTexture = null;

    public int ScreenWidth = 0;
    public int ScreenHeight = 0;

    private bool ScreenCaptureWithUI_Ing = false;
    private bool ScreenCaptureWithoutUI_Ing = false;

    private string Day;
    public string FilePath;
    public string FolderName;
    public string FileName;

    public Image FlashImg;
    public float FlashDuration = 0.5f;
    public AnimationCurve FlashCurve;
    private Coroutine FlashCoroutine;

    private bool ImmediatelySave = false;

    public void Awake()
    {
        ScreenCaptureTexture = null;
#if UNITY_EDITOR
        FilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
#elif UNITY_ANDROID
        FilePath = "storage/emulated/0/DCIM/";
#endif
        FolderName =  Application.productName;
    }

    #region FuncScreenCapture
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
        if (FlashImg != null)
        {
            if (FlashCoroutine != null)
            {
                return;
            }
            else
            {
                FlashCoroutine = StartCoroutine(Co_Flash(FlashDuration));
            }
        }
        SetFileName();

        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;

        ScreenCaptureTexture = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.RGB24, false);
        ScreenCaptureTexture.ReadPixels(new Rect(0, 0, ScreenWidth, ScreenHeight), 0, 0);

        if (UseWatermark)
        {
            WatermarkScaleFactor = ScreenWidth / 2400f;
            ScreenCaptureTexture = ScreenCaptureTexture.AddWaterMark(WatermarkTexture, WatermarkAnchor, WatermarkScaleFactor, WatermarkOffsetX, WatermarkOffsetY);
        }
        else
        {
            ScreenCaptureTexture.Apply();
        }

        if(ImmediatelySave)
        {
            Save();
        }
    }
    #endregion

    IEnumerator Co_Flash(float duration)
    {
        FlashImg.color = Color.white;

        for (float time = 0; time < duration; time+=Time.deltaTime)
        {
            Color currentColor = FlashImg.color;
            currentColor.a = Mathf.Lerp(1, 0, FlashCurve.Evaluate(time/duration));
            FlashImg.color = currentColor;
            yield return null;
        }
        FlashImg.color = Color.clear;
        FlashCoroutine = null;
    }

    #region SetOptions
    public void SetUseWatermark(bool value)
    {
        UseWatermark = value;
    }
    public void SetWithUI(bool value)
    {
        WithUI = value;
    }
    public void SetWatermarkAnchor(TextureExtensions.Watermark_Anchor watermark_Anchor)
    {
        WatermarkAnchor = watermark_Anchor;
    }
    #endregion

    public void OnClickScreenCapture()
    {
#if UNITY_EDITOR
        DoScreenCapture();
#elif UNITY_ANDROID
        AndroidPermission.CheckPermissionAndDo(Permission.ExternalStorageWrite, () => DoScreenCapture(), denidedAndDonAskAgain: AndroidPermission.OpenAppSetting);
#else
        DoScreenCapture();
#endif
    }

    private void DoScreenCapture()
    {
        ImmediatelySave = true;
        ScreenCaptureWithoutUI(true);
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
        ToastMessage.Instance.ShowToast($"Storage Path : {FilePath}/{FolderName}", ToastLength.Short);

#if !UNITY_EDITOR
#if UNITY_ANDROID
        GGAndroid.RefreshGallery(FilePath, FolderName);
#endif
#endif
        ImmediatelySave = false;
    }
}
