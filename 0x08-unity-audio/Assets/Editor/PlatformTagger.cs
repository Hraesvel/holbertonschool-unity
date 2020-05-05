using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlatformTagger))]
public class PlatformTagger : EditorWindow
{
    private Editor editor;
    public GameObject source;
    [SerializeField] private List<string> TagList = new List<string>();

    private bool _rand;

    [MenuItem("Window/Platform Tagger")]
    private static void ShowWindow()
    {
        GetWindow<PlatformTagger>("Platform Tagger");
    }

    private void OnGUI()
    {
        if (!editor)
            editor = Editor.CreateEditor(this);

        TagList.Add("GrassPlatform");
        TagList.Add("RockPlatform");

        GUILayout.Label("Parent GameObject");
        source = (GameObject) EditorGUILayout.ObjectField(source, typeof(GameObject), true);
        GUILayout.Space(20f);
        GUILayout.Label("Options");
        _rand = EditorGUILayout.Toggle("Randomize tags", _rand);
        GUILayout.Space(20f);
        GUILayout.Label("Tags");
        if (editor)
            editor.OnInspectorGUI();

        GUILayout.Space(20f);


        if (GUILayout.Button("Batch"))
            foreach (var parent in source.GetComponentsInChildren<Transform>())
            {
                if (parent.childCount > 0)
                {
                    // Debug.Log(parent.name);
                    var child = parent.GetChild(0);
                    if (child.gameObject.layer == LayerMask.NameToLayer("Platform"))
                    {
                        var tagIdx = 0;
                        if (_rand)
                            tagIdx = (int) Random.Range(0f, TagList.Count);
                        child.gameObject.tag = TagList[tagIdx];
                        
                    }
                    else
                        child.gameObject.tag = "Untagged";
                }
            }
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }


    [CustomEditor(typeof(PlatformTagger), true)]
    public class ListEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var list = serializedObject.FindProperty("TagList");
            EditorGUILayout.PropertyField(list, new GUIContent("Tags to assign"), true);
        }
    }
}