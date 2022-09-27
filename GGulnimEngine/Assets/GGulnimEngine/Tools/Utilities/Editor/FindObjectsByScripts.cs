using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class FindObjectsByScripts : EditorWindow
{
    protected List<GameObject> _resultsList;
    protected Vector2 _scrollView;
    protected int _selected = -1;
    protected int _searchCount = 0;

    protected MonoScript _searchedMonoBehaviour;
	protected MonoScript _lastSearchedMonoBehaviour;
	protected string _searchedMonoBehaviourName = "";

	private Color _originalBackgroundColor;
    [MenuItem("GGulnimEngine/Utilities/Find Objects/Find Objects By Scripts")]
    public static void MenuAction()
    {
        OpenWindow();
    }

    public static void OpenWindow()
    {
        FindObjectsByScripts window = GetWindow<FindObjectsByScripts>();
        window.titleContent = new GUIContent("Find Objects By Scripts");
        window.Show();
    }

    private void OnEnable()
    {
        _resultsList = new List<GameObject>();
        _scrollView = Vector2.zero;
    }

    private void OnGUI()
    {
        DrawSearchScript();
        if (_searchedMonoBehaviour == null)
        {
            return;
        }
        DrawHeader();
        DrawResultList();
    }

    private void DrawSearchScript()
    {
        _originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = ColorExtensions.DeepSkyBlue;
        GUI.skin.box.padding = new RectOffset(10, 10, 10, 10);
        GUILayout.BeginHorizontal("box");
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        GUILayout.Label("Select a Script to search for:");
        _searchedMonoBehaviour = (MonoScript)EditorGUILayout.ObjectField(_searchedMonoBehaviour, typeof(MonoScript), false);
        GUILayout.EndVertical();
        GUILayout.Space(10);

        if (_searchedMonoBehaviour != _lastSearchedMonoBehaviour)
        {
            _lastSearchedMonoBehaviour = _searchedMonoBehaviour;
            if (_searchedMonoBehaviour != null)
            {
                _searchedMonoBehaviourName = _searchedMonoBehaviour.name;
            }
            AssetDatabase.SaveAssets();
        }
        GUILayout.EndHorizontal();
        GUI.backgroundColor = _originalBackgroundColor;
    }
    private void DrawHeader()
    {
        GUI.backgroundColor = ColorExtensions.DeepSkyBlue;
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
            GUILayout.Label($"No GameObjects have \"{_searchedMonoBehaviourName}\"", EditorStyles.boldLabel);
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
        string searchedMonoBehaviourPath = AssetDatabase.GetAssetPath(_searchedMonoBehaviour);
        _resultsList = new List<GameObject>();
        foreach (string prefab in allPrefabsInProject)
        {
            string[] pathName = new string[] { prefab };
            string[] monoDependenciesPaths = AssetDatabase.GetDependencies(pathName, true);
            foreach (string monoDependencyPath in monoDependenciesPaths)
            {
                if (monoDependencyPath == searchedMonoBehaviourPath)
                {
                    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(prefab);
                    _resultsList.Add(obj);
                }
            }
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
                if (obj.transform.GetComponent(_searchedMonoBehaviour.GetClass()))
                {
                    _resultsList.Add(obj);
                }
            }
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
