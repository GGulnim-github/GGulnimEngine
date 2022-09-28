using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WatermarkScreenCapture))]
public class WatermarkScreenCaptureEditor : Editor
{
    private WatermarkScreenCapture _target;

    private WatermarkScreenCapture Target
    {
        get
        {
            if (_target != null) return _target;
            _target = (WatermarkScreenCapture)target;
            return _target;
        }
    }

    private Color _originalBackgroundColor;

    private SerializedProperty WatermarkTexture;
    private SerializedProperty WatermarkAnchor;
    private SerializedProperty WatermarkScaleFactor;
    private SerializedProperty WatermarkOffsetX;
    private SerializedProperty WatermarkOffsetY;

    private SerializedProperty FlashImg;
    private SerializedProperty FlashDuration;
    private SerializedProperty FlashCurve;

    private void OnEnable()
    {
        _target = (WatermarkScreenCapture)target;

        WatermarkTexture = serializedObject.FindProperty("WatermarkTexture");
        WatermarkAnchor = serializedObject.FindProperty("WatermarkAnchor");
        WatermarkScaleFactor = serializedObject.FindProperty("WatermarkScaleFactor");
        WatermarkOffsetX = serializedObject.FindProperty("WatermarkOffsetX");
        WatermarkOffsetY = serializedObject.FindProperty("WatermarkOffsetY");

        FlashImg = serializedObject.FindProperty("FlashImg");
        FlashDuration = serializedObject.FindProperty("FlashDuration");
        FlashCurve = serializedObject.FindProperty("FlashCurve");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.Label("Options", EditorStyles.boldLabel);

        DrawPropertyField(WatermarkTexture, ColorExtensions.DeepSkyBlue);
        DrawPropertyField(WatermarkAnchor, ColorExtensions.DeepSkyBlue);
        DrawPropertyField(WatermarkScaleFactor, ColorExtensions.DeepSkyBlue);
        DrawPropertyField(WatermarkOffsetX, ColorExtensions.DeepSkyBlue);
        DrawPropertyField(WatermarkOffsetY, ColorExtensions.DeepSkyBlue);

        GUILayout.Space(10);
        GUILayout.Label("Flash", EditorStyles.boldLabel);
        DrawPropertyField(FlashImg, ColorExtensions.DeepSkyBlue);
        DrawPropertyField(FlashDuration, ColorExtensions.DeepSkyBlue);
        DrawPropertyField(FlashCurve, ColorExtensions.DeepSkyBlue);

        GUILayout.Space(10);
        if (Application.isPlaying)
        {
            GUILayout.Label("Use WaterMark", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            DrawCaptureButton("Screen Capture (With UI)", ColorExtensions.DeepSkyBlue, Target.ScreenCaptureWithUI, true, EditorStyles.miniButtonLeft);
            DrawCaptureButton("Screen Capture (Without UI)", ColorExtensions.DodgerBlue, Target.ScreenCaptureWithoutUI, true, EditorStyles.miniButtonRight);
            EditorGUILayout.EndHorizontal();
            GUILayout.Label("Dont Use WaterMark", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            DrawCaptureButton("Screen Capture (With UI)", ColorExtensions.DeepSkyBlue, Target.ScreenCaptureWithUI, false, EditorStyles.miniButtonLeft);
            DrawCaptureButton("Screen Capture (Without UI)", ColorExtensions.DodgerBlue, Target.ScreenCaptureWithoutUI, false, EditorStyles.miniButtonRight);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Result", EditorStyles.boldLabel);
            if (Target.ScreenCaptureTexture)
            {
                GUILayout.Label(Target.ScreenCaptureTexture, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(300));
                GUILayout.FlexibleSpace();
                GUILayout.Label($"{Target.FileName} ({Target.ScreenWidth}x{Target.ScreenHeight})");
                GUILayout.Label($"Storage Path : {Target.FilePath}/{Target.FolderName}");
                DrawSaveButton("Save", ColorExtensions.CornflowerBlue, Target.Save);
            }
        }
        else
        {
            GUILayout.Label("Can Capture After Playing", EditorStyles.boldLabel);
            GUILayout.Label("If an error occurs after capturing, change the game view to full screen");
        }
        serializedObject.ApplyModifiedProperties();
    }


    public void DrawPropertyField(SerializedProperty property, Color backgroundColor)
    {
        _originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = backgroundColor;
        EditorGUILayout.PropertyField(property);
        GUI.backgroundColor = _originalBackgroundColor;
    }

    public void DrawCaptureButton(string buttonLabel, Color buttonColor, Action<bool> action, bool useWatermark, GUIStyle styles)
    {
        _originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = buttonColor;
        if (GUILayout.Button(buttonLabel, styles))
        {
            action.Invoke(useWatermark);
        }
        GUI.backgroundColor = _originalBackgroundColor;
    }
    public void DrawSaveButton(string buttonLabel, Color buttonColor, Action action)
    {
        _originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = buttonColor;
        if (GUILayout.Button(buttonLabel))
        {
            action.Invoke();
        }
        GUI.backgroundColor = _originalBackgroundColor;
    }
}
