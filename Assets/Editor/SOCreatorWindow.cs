using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public class SOCreatorWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private List<Type> allUserSoTypes;
    private List<Type> filteredSoTypes;
    private string searchFilter = "";

    [MenuItem("Assets/SO Creator")]
    public static void ShowWindow()
    {
        GetWindow<SOCreatorWindow>("SO Creator");
    }

    private void OnEnable()
    {
        FindAllUserScriptableObjectTypes();
        ApplyFilter();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Please select type of scriptable object want to create:", EditorStyles.boldLabel);
        
        EditorGUI.BeginChangeCheck();
        searchFilter = EditorGUILayout.TextField("filter(class name or namespace)", searchFilter);
        if (EditorGUI.EndChangeCheck())
        {
            ApplyFilter();
        }

        EditorGUILayout.Space(5);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (filteredSoTypes == null || filteredSoTypes.Count == 0)
        {
            EditorGUILayout.HelpBox("no result", MessageType.Info);
        }
        else
        {
            foreach (var type in filteredSoTypes)
            {
                string buttonLabel = string.IsNullOrEmpty(type.Namespace) ? type.Name : $"{type.Namespace} / {type.Name}";
                if (GUILayout.Button(buttonLabel))
                {
                    CreateSOAsset(type);
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }


    private void FindAllUserScriptableObjectTypes()
    {
        allUserSoTypes = new List<Type>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            try
            {
                allUserSoTypes.AddRange(assembly.GetTypes().Where(t =>
                    t.IsSubclassOf(typeof(ScriptableObject)) &&
                    !t.IsAbstract &&
                    (t.Namespace == null || (!t.Namespace.StartsWith("UnityEngine") && !t.Namespace.StartsWith("UnityEditor"))) &&
                    t.GetCustomAttribute<CreateAssetMenuAttribute>() == null
                ).ToList());
            }
            catch (ReflectionTypeLoadException) { continue; }
        }
        
        allUserSoTypes = allUserSoTypes.OrderBy(t => t.Name).ToList();
    }
    
    private void ApplyFilter()
    {
        if (string.IsNullOrEmpty(searchFilter))
        {
            filteredSoTypes = allUserSoTypes;
        }
        else
        {
            string lowerFilter = searchFilter.ToLower();
            filteredSoTypes = allUserSoTypes.Where(t =>
                t.Name.ToLower().Contains(lowerFilter) ||
                (t.Namespace != null && t.Namespace.ToLower().Contains(lowerFilter))
            ).ToList();
        }
    }
    
    private void CreateSOAsset(Type type)
    {
        ScriptableObject asset = CreateInstance(type);

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if (System.IO.Path.GetExtension(path) != "")
        {
            path = System.IO.Path.GetDirectoryName(path);
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/New {type.Name}.asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        
        this.Close();
    }
}