using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class FindObejctsWithMissingScripts : EditorWindow
{
    protected List<GameObject> _resultsList;    
    protected Vector2 _scrollView;
    protected int _selected = -1;
    protected int _searchCount = 0;

    private Color _originalBackgroundColor;
    [MenuItem("GGulnimEngine/Utilities/Find Objects/Find Objects With Missing Scripts")]
    public static void MenuAction()
    {
        OpenWindow();
    }
    public static void OpenWindow()
    {
        FindObejctsWithMissingScripts window = GetWindow<FindObejctsWithMissingScripts>();
        window.position = new Rect(400, 400, 500, 300);
        window.titleContent = new GUIContent("Find Objects With Missing Scripts");
        window.Show();
    }

    private void OnEnable()
    {
        _resultsList = new List<GameObject>();
        _scrollView = Vector2.zero;
    }

    private void OnGUI()
    {
        DrawHeader();
        DrawResultList();
    }
    private void DrawHeader()
    {
        _originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = ColorExtensions.DeepSkyBlue;
        GUI.skin.box.padding = new RectOffset(10, 10, 10, 10);
        EditorGUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Find in Assets"))
        {
            FindInAssets();
        }
        if (GUILayout.Button("Find in Current Scene"))
        {
            FindInScenes();
        }
        EditorGUILayout.EndHorizontal();
        GUI.backgroundColor = _originalBackgroundColor;
    }
    private void DrawResultList()
    {
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
        if (_resultsList.Count == 0)
        {
            GUILayout.Label("No GameObjects have missing components.", EditorStyles.boldLabel);
            return;
        }

        GUI.backgroundColor = ColorExtensions.DeepSkyBlue;
        _scrollView = EditorGUILayout.BeginScrollView(_scrollView);
        for (int i = 0; i < _resultsList.Count; i++)
        {
            if (_resultsList[i] == null)
            {
                _resultsList.Remove(_resultsList[i]);
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
                        buttonStr = AssetDatabase.GetAssetPath(_resultsList[i]);
                        break;
                    case 1:
                        buttonStr = GetFullPath(_resultsList[i].transform);
                        break;
                }
                if (GUILayout.Button(buttonStr))
                {
                    EditorGUIUtility.PingObject(_resultsList[i]);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        GUI.backgroundColor = _originalBackgroundColor;
    }

    private void FindInAssets()
    {
        _selected = 0;
        _resultsList.Clear();
        
        string[] allPrefabsInProject = AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".prefab")).ToArray();
        _searchCount = allPrefabsInProject.Length;

        foreach (string prefab in allPrefabsInProject)
        {
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(prefab);
            RecursiveDepthSearch(obj);
        }
    }

    private void FindInScenes()
    {
        _selected = 1;
        _resultsList.Clear();

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
                if (!_resultsList.Contains(root))
                    _resultsList.Add(root);
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