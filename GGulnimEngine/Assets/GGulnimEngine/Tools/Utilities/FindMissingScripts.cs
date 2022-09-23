
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class FindMissingScripts : EditorWindow
{
    protected List<GameObject> _objectWithMissingScripts;    
    protected Vector2 _scrollView;
    protected int _selected = -1;
    protected int _searchCount = 0;

    private Color _originalBackgroundColor;
    [MenuItem("GGUlnimEngine/Utilities/Find Missing Component")]
    public static void MenuAction()
    {
        OpenWindow();
    }
    public static void OpenWindow()
    {
        FindMissingScripts window = GetWindow<FindMissingScripts>();
        window.position = new Rect(400, 400, 500, 300);
        window.titleContent = new GUIContent("Find Missing Scripts");
        window.Show();
    }

    private void OnEnable()
    {
        _objectWithMissingScripts = new List<GameObject>();
        _scrollView = Vector2.zero;
    }

    private void OnGUI()
    {
        _originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = ColorExtensions.FireBrick;
        GUI.skin.box.padding = new RectOffset(10, 10, 10, 10);
        EditorGUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Find in Assets"))
            FindInAssets();
        if (GUILayout.Button("Find in Current Scene"))
            FindInScenes();
        EditorGUILayout.EndHorizontal();

        switch (_selected)
        {
            case 0:
                GUILayout.Space(5);
                GUILayout.Label($"Check {_searchCount} GameObjects in Assets");
                break;
            case 1:
                GUILayout.Space(5);
                GUILayout.Label($"Check {_searchCount} GameObjects in Current Scene");
                break;
        }

        GUILayout.Space(5);
        GUILayout.Label("Result", EditorStyles.boldLabel);
        if (_objectWithMissingScripts.Count == 0)
        {
            GUILayout.Label("No GameObjects have missing components.", EditorStyles.boldLabel);
            GUI.backgroundColor = _originalBackgroundColor;
            return;
        }
        _scrollView = EditorGUILayout.BeginScrollView(_scrollView);
        for(int i = 0; i < _objectWithMissingScripts.Count; i++)
        {
            if (_objectWithMissingScripts[i] == null)
            {
                _objectWithMissingScripts.Remove(_objectWithMissingScripts[i]);
                i--;
                continue;
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                string buttonStr = string.Empty;
                switch (_selected)
                {
                    case 0:
                        buttonStr = AssetDatabase.GetAssetPath(_objectWithMissingScripts[i]);
                        break;
                    case 1:
                        buttonStr = GetFullPath(_objectWithMissingScripts[i].transform);
                        break;
                }
                if (GUILayout.Button(buttonStr))
                {
                    EditorGUIUtility.PingObject(_objectWithMissingScripts[i]);
                }
                EditorGUILayout.EndHorizontal();
            }
        }  
        EditorGUILayout.EndScrollView();
        GUI.backgroundColor = _originalBackgroundColor;
    }

    void FindInAssets()
    {
        _selected = 0;
        _objectWithMissingScripts.Clear();
        
        string[] assetGUIDs = AssetDatabase.FindAssets("t:GameObject");
        
        _searchCount = assetGUIDs.Length;

        foreach (string assetGuiD in assetGUIDs)
        {
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetGuiD));
            RecursiveDepthSearch(obj);
        }
    }

    private void FindInScenes()
    {
        _selected = 1;
        _objectWithMissingScripts.Clear();

        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            var rootGOs = SceneManager.GetSceneAt(i).GetRootGameObjects();

            _searchCount = rootGOs.Length;

            foreach (GameObject obj in rootGOs)
            {
                RecursiveDepthSearch(obj);
            }
        }
    }

    private void RecursiveDepthSearch(GameObject root)
    {
        Component[] components = root.GetComponents<Component>();
        foreach (Component c in components)
        {
            if (c == null)
            {
                if (!_objectWithMissingScripts.Contains(root))
                    _objectWithMissingScripts.Add(root);
            }
        }
        foreach (Transform t in root.transform)
        {
            RecursiveDepthSearch(t.gameObject);
        }
    }

    private string GetFullPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = $"{transform.name} / {path}";
        }
        return path;
    }
}