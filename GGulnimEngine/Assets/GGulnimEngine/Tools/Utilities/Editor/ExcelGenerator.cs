using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ExcelGenerator : EditorWindow
{
    protected string _currentFilePath = string.Empty;
    protected bool _invalidExceFile;
    
    protected string[] _folders;
    protected int _selectFolderIndex = -1;
    protected string _currentSavePath = string.Empty;

    private Color _originalBackgroundColor;
    [MenuItem("GGulnimEngine/Utilities/Excel/Excel Generator")]
    public static void MenuAction()
    {
        OpenWindow();
    }
    public static void OpenWindow()
    {
        ExcelGenerator window = GetWindow<ExcelGenerator>();
        window.titleContent = new GUIContent("Excel Generator");
        window.Show();
    }
    private void OnFocus()
    {
        List<string> folders = new List<string>();
        Queue<string> toCheckFolders = new Queue<string>();
        toCheckFolders.Enqueue("Assets");
        while (toCheckFolders.Count > 0)
        {
            string folder = toCheckFolders.Dequeue();
            folders.Add(folder);
            string[] subFolders = AssetDatabase.GetSubFolders(folder);
            for (int i = 0, imax = subFolders.Length; i < imax; i++)
            {
                toCheckFolders.Enqueue(subFolders[i].Replace('\\', '/'));
            }
        }
        _folders = folders.ToArray();
    }

    private void OnGUI()
    {
        _originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = ColorExtensions.DeepSkyBlue;
        GUI.skin.box.padding = new RectOffset(10, 10, 10, 10);
        GUILayout.BeginVertical("box");
        if (GUILayout.Button("Select File"))
        {
            _currentFilePath= EditorUtility.OpenFilePanel("Select Excel File", ".", "xlsx");
            _invalidExceFile = string.IsNullOrEmpty(_currentFilePath);
        }
        GUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(_invalidExceFile);
        if (GUILayout.Button("Open File"))
        {
            EditorUtility.OpenWithDefaultApp(_currentFilePath);
        }
        if(GUILayout.Button("Show In Explorer"))
        {
            string folder = Path.GetDirectoryName(_currentFilePath);
            EditorUtility.OpenWithDefaultApp(folder);
        }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUI.backgroundColor = _originalBackgroundColor;

        if (string.IsNullOrEmpty(_currentFilePath))
        {
            return;
        }
        GUILayout.Label("Current File :", EditorStyles.boldLabel);
        GUILayout.Label(_currentFilePath);
        int selectIndex = EditorGUILayout.Popup("Script Directory", _selectFolderIndex, _folders);
        if (selectIndex != _selectFolderIndex)
        {
            _selectFolderIndex = selectIndex;
            _currentSavePath = _folders[_selectFolderIndex];
            GUILayout.Label(_currentSavePath);
        }
    }
}
