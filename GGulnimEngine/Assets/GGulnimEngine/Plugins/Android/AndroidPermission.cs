using System;
using UnityEngine;
using UnityEngine.Android;

public static class AndroidPermission
{
    // ī�޶�
    public static void RequestCamera()
    {
        RequestPermission(Permission.Camera);
    }
    // ����ũ
    public static void RequestMicrophonee()
    {
        RequestPermission(Permission.Microphone);
    }
    // ��Ȯ�� ��ġ(GPS, ��Ʈ��ũ ��� ���)
    public static void RequestFineLocation()
    {
        RequestPermission(Permission.FineLocation);
    }
    // ����Ȯ�� ��ġ(��Ʈ��ũ�� ���)
    public static void RequestCoarseLocation()
    {
        RequestPermission(Permission.CoarseLocation);
    }
    // �ܺ� ����ҿ��� �б�
    public static void RequestExternalStorageWrite()
    {
        RequestPermission(Permission.ExternalStorageWrite);
    }
    // �ܺ� ����ҿ� ����
    public static void RequestExternalStorageRead()
    {
        RequestPermission(Permission.ExternalStorageRead);
    }

    // ���� ��û
    public static void RequestPermission(string permission)
    {
        if (!Permission.HasUserAuthorizedPermission(permission))
        {
            Permission.RequestUserPermission(permission);
        }
    }

    // ������ �ʿ��� ����� ������ �� �� �� �� ������ üũ�ϰ� ����� �����Ͽ��� �Ѵ�.
    public static void CheckPermissionAndDo(string permission, Action granted = null, Action denided = null, Action denidedAndDonAskAgain = null)
    {
        // ���� ������ �ȵ� ����
        if (Permission.HasUserAuthorizedPermission(permission) == false)
        {
            // ���� ��û ���信 ���� ���� �ݹ�
            PermissionCallbacks pCallbacks = new PermissionCallbacks();
            pCallbacks.PermissionGranted += _ => granted(); // ���� �� ��� ����
            pCallbacks.PermissionDenied += _ => denided(); // ���� �� ��� ����
            pCallbacks.PermissionDeniedAndDontAskAgain += _ => denidedAndDonAskAgain(); // ����� ���� �� ��� ����(�� �� �����ϸ� ���� ���� â�� ����� �ʱ� ����)

            // ���� ��û
            Permission.RequestUserPermission(permission, pCallbacks);
        }
        // ������ ���� �Ǿ� �ִ� ���
        else
        {
            granted(); // �ٷ� ��� ����
        }
    }

    // �� ���� ȭ��
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


