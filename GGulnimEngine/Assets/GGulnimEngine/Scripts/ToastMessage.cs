using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToastLength { Short, Long }
#if UNITY_EDITOR
public class ToastMessage : Singleton<ToastMessage>
{
    private string Message = string.Empty;
    private float Length = 0f;
    private GUIStyle ToastStyle;


    private void Update()
    {
        if (Length > 0f)
            Length -= Time.unscaledDeltaTime;
    }
    private void OnGUI()
    {
        if (Length <= 0f) return;

        float width = Screen.width * 0.8f;
        float height = Screen.height * 0.08f;
        Rect rect = new Rect((Screen.width - width) * 0.5f, Screen.height * 0.8f, width, height);

        if (ToastStyle == null)
        {
            ToastStyle = new GUIStyle(GUI.skin.box);
            ToastStyle.fontStyle = FontStyle.Bold;
            ToastStyle.alignment = TextAnchor.MiddleCenter;
            ToastStyle.normal.textColor = Color.white;
        }
        ToastStyle.fontSize =  Screen.width/60;

        GUI.Box(rect, Message, ToastStyle);
    }


    public void ShowToastShort(string message)
    {
        ShowToast(message, ToastLength.Short);
    }
    public void ShowToastLong(string message)
    {
        ShowToast(message, ToastLength.Long);
    }

    public void ShowToast(string message, ToastLength length)
    {
        Message = message;

        switch (length)
        {
            case ToastLength.Short:
                Length = 2f;
                break;
            case ToastLength.Long:
                Length = 3.5f;
                break;
        }
    }
}
#endif