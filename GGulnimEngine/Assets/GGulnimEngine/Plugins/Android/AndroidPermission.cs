using System;
using UnityEngine;
using UnityEngine.Android;

public static class AndroidPermission
{
    // 카메라
    public static void RequestCamera()
    {
        RequestPermission(Permission.Camera);
    }
    // 마이크
    public static void RequestMicrophonee()
    {
        RequestPermission(Permission.Microphone);
    }
    // 정확한 위치(GPS, 네트워크 모두 사용)
    public static void RequestFineLocation()
    {
        RequestPermission(Permission.FineLocation);
    }
    // 부정확한 위치(네트워크만 사용)
    public static void RequestCoarseLocation()
    {
        RequestPermission(Permission.CoarseLocation);
    }
    // 외부 저장소에서 읽기
    public static void RequestExternalStorageWrite()
    {
        RequestPermission(Permission.ExternalStorageWrite);
    }
    // 외부 저장소에 쓰기
    public static void RequestExternalStorageRead()
    {
        RequestPermission(Permission.ExternalStorageRead);
    }

    // 권한 요청
    public static void RequestPermission(string permission)
    {
        if (!Permission.HasUserAuthorizedPermission(permission))
        {
            Permission.RequestUserPermission(permission);
        }
    }

    // 권한이 필요한 기능을 수행할 때 한 번 더 권한을 체크하고 기능을 수행하여야 한다.
    public static void CheckPermissionAndDo(string permission, Action granted = null, Action denided = null, Action denidedAndDonAskAgain = null)
    {
        // 권한 승인이 안된 상태
        if (Permission.HasUserAuthorizedPermission(permission) == false)
        {
            // 권한 요청 응답에 따른 동작 콜백
            PermissionCallbacks pCallbacks = new PermissionCallbacks();
            pCallbacks.PermissionGranted += _ => granted(); // 승인 시 기능 실행
            pCallbacks.PermissionDenied += _ => denided(); // 거절 시 기능 실행
            pCallbacks.PermissionDeniedAndDontAskAgain += _ => denidedAndDonAskAgain(); // 물어보지 않을 때 기능 실행(두 번 거절하면 권한 묻는 창을 띄우지 않기 때문)

            // 권한 요청
            Permission.RequestUserPermission(permission, pCallbacks);
        }
        // 권한이 승인 되어 있는 경우
        else
        {
            granted(); // 바로 기능 실행
        }
    }

    // 앱 설정 화면
    public static void OpenAppSetting()
    {
        try
        {
            using AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            string packageName = unityActivity.Call<string>("getPackageName");
            using AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            using AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null);
            using AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject);
            intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
            intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
            unityActivity.Call("startActivity", intentObject);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}


