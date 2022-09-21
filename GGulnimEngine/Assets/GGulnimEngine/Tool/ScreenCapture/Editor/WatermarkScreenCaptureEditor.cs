using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WatermarkScreenCapture))]
public class WatermarkScreenCaptureEditor : Editor
{
    protected WatermarkScreenCapture _watermarkScreenCapture;

    public override void OnInspectorGUI()
    {
        _watermarkScreenCapture = (WatermarkScreenCapture)target;
        base.OnInspectorGUI();

        GUILayout.Space(10);

        if(Application.isPlaying)
        {
            GUILayout.Label("Capture", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("With UI", EditorStyles.miniButtonLeft))
            {
                _watermarkScreenCapture.DoScreenCaptureWithUI();
            }
            if (GUILayout.Button("Without UI", EditorStyles.miniButtonLeft))
            {
                _watermarkScreenCapture.DoScreenCaptureWithoutUI();
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("Result", EditorStyles.boldLabel);
            if (_watermarkScreenCapture.ScreenCaptureTexture)
            {
                GUILayout.Label(_watermarkScreenCapture.ScreenCaptureTexture, GUILayout.ExpandWidth(true), GUILayout.Height(300));
            }
        }
    }
}
