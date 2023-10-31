using UnityEditor;
using UnityEngine;

namespace _Main.Scripts.Editor
{
    public class ReplaceWithPrefab : EditorWindow
    {
        [SerializeField] private GameObject prefab;

        [MenuItem("Tools/Replace With Prefab")]
        static void CreateReplaceWithPrefab()
        {
            EditorWindow.GetWindow<ReplaceWithPrefab>();
        }

        private void OnGUI()
        {
            prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

            if (GUILayout.Button("Replace"))
            {
                var l_selection = Selection.gameObjects;

                for (var l_i = l_selection.Length - 1; l_i >= 0; --l_i)
                {
                    var l_selected = l_selection[l_i];
                    var l_prefabType = PrefabUtility.GetPrefabType(prefab);
                    GameObject l_newObject;

                    if (l_prefabType == PrefabType.Prefab)
                    {
                        l_newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    }
                    else
                    {
                        l_newObject = Instantiate(prefab);
                        l_newObject.name = prefab.name;
                    }

                    if (l_newObject == null)
                    {
                        Debug.LogError("Error instantiating prefab");
                        break;
                    }

                    Undo.RegisterCreatedObjectUndo(l_newObject, "Replace With Prefabs");
                    l_newObject.transform.parent = l_selected.transform.parent;
                    l_newObject.transform.localPosition = l_selected.transform.localPosition;
                    l_newObject.transform.localRotation = l_selected.transform.localRotation;
                    l_newObject.transform.localScale = l_selected.transform.localScale;
                    l_newObject.transform.SetSiblingIndex(l_selected.transform.GetSiblingIndex());
                    Undo.DestroyObjectImmediate(l_selected);
                }
            }

            GUI.enabled = false;
            EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
        }
    }
}