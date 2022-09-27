using System;
using System.IO;
using UnityEngine;

public static class GGAndroid
{
    public static void ShowToast(string message, ToastLength length)
    {
        AndroidJavaClass plugin = new AndroidJavaClass("com.ggulnimstudio.unity.UnityPlugin");
        AndroidJavaObject instance = plugin.CallStatic<AndroidJavaObject>("getInstance");

        switch(length)
        {
            case ToastLength.Short:
                instance.Call("showToastShort", message);
                break;
            case ToastLength.Long:
                instance.Call("showToastLong", message);
                break;
        }
    }

    public static void RefreshGallery(string filePath, string folderName)
    {
        string directory = Path.Combine(filePath, folderName);
        try
        {
            using AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaObject contextObject = unityActivity.Call<AndroidJavaObject>("getApplicationContext");
            using AndroidJavaClass mediaScannerConnectionClass = new AndroidJavaClass("android.media.MediaScannerConnection");
            using AndroidJavaClass environmentClass = new AndroidJavaClass("android.os.Environment");
            using AndroidJavaObject exDirOjbect = environmentClass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory");
            mediaScannerConnectionClass.CallStatic("scanFile", contextObject, new string[] { directory }, null, null);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}


