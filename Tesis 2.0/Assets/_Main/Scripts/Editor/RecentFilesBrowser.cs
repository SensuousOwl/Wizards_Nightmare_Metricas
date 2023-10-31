using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Main.Scripts.Editor
{
    public class RecentFilesBrowser : EditorWindow
    {
        private const string USER_FACING_NAME = "Recent Files Browser";

        private static Texture m_favoriteTexture;
        private static SerializedObject m_serializedData;

        private readonly string m_subDirectory = Path.Combine("_PsychoKitties", "Editor", "untracked");
        private string SaveDirectory => Path.Combine("Assets", m_subDirectory);
        private string SavePath => Path.Combine(SaveDirectory, "RecentFilesBrowserData.asset");

        private SerializedObject Data => InitData();

        private SerializedProperty FavoriteSelections => Data.FindProperty("favoriteSelections");
        private SerializedProperty PreviousSelections => Data.FindProperty("previousSelections");
        private SerializedProperty MaxItems => Data.FindProperty("maxItems");
        private SerializedProperty ScrollPos => Data.FindProperty("scrollPos");

        [MenuItem("Tools/" + USER_FACING_NAME)]
        public static void ShowWindow()
        {
            EditorWindow l_window = GetWindow(typeof(RecentFilesBrowser));
            l_window.minSize = new Vector2(100, 80);
            l_window.maxSize = new Vector2(400, 800);
            l_window.titleContent = new GUIContent(USER_FACING_NAME);
            l_window.Show();
        }

        private SerializedObject InitData()
        {
            if (m_serializedData?.targetObject == null)
            {
                RecentFilesBrowserData l_data = AssetDatabase.LoadAssetAtPath<RecentFilesBrowserData>(SavePath);
                if (l_data == null)
                {
                    Directory.CreateDirectory(Path.Combine(Application.dataPath, m_subDirectory));
                    l_data = CreateInstance<RecentFilesBrowserData>();
                    AssetDatabase.CreateAsset(l_data, SavePath);
                    AssetDatabase.SaveAssets();
                }

                if (m_serializedData?.targetObject != l_data)
                {
                    m_serializedData = new SerializedObject(l_data);
                }

                CleanUpInvalidSelections();
            }

            return m_serializedData;
        }

        private void OnGUI()
        {
            if (!m_favoriteTexture)
            {
                m_favoriteTexture = EditorGUIUtility.IconContent("d_Favorite").image;
            }

            ScrollPos.vector2Value = EditorGUILayout.BeginScrollView(ScrollPos.vector2Value);

            DrawFavoriteSelections();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            if (PreviousSelections.arraySize == 0)
            {
                DrawInstructions();
            }
            else
            {
                DrawSelections();
            }

            EditorGUILayout.EndScrollView();

            DrawControls();

            Data.ApplyModifiedProperties();
        }

        private void DrawInstructions()
        {
            EditorGUILayout.BeginVertical("HelpBox");

            GUIStyle l_labelStyle = new GUIStyle(EditorStyles.label);
            l_labelStyle.alignment = TextAnchor.MiddleCenter;
            l_labelStyle.wordWrap = true;
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField($"Items selected in the Project window will show up here.", l_labelStyle);
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            EditorGUILayout.EndVertical();
        }

        private void DrawControls()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField($"Max Items: {MaxItems.intValue}", GUILayout.MaxWidth(80));

            if (GUILayout.Button("+", GUILayout.MaxWidth(40)))
            {
                MaxItems.intValue++;
            }

            EditorGUI.BeginDisabledGroup(MaxItems.intValue == 0);

            if (GUILayout.Button("-", GUILayout.MaxWidth(40)))
            {
                MaxItems.intValue--;
                while (PreviousSelections.arraySize > MaxItems.intValue)
                {
                    PreviousSelections.RemoveAt(0);
                }
            }

            EditorGUI.EndDisabledGroup();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFavoriteSelections()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Clean Up", GUILayout.Width(80)))
            {
                CleanUpInvalidSelections();
            }

            if (FavoriteSelections.arraySize > 0 && GUILayout.Button("Clear", GUILayout.Width(50)))
            {
                ClearFavoriteSelections();
            }

            EditorGUILayout.EndHorizontal();
            for (int l_index = FavoriteSelections.arraySize - 1; l_index >= 0; l_index--)
            {
                Object l_selection = FavoriteSelections.ElementAt(l_index).objectReferenceValue;
                DrawSelection(l_selection, true);
            }
        }

        private void DrawSelections()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (PreviousSelections.arraySize > 0 && GUILayout.Button("Clear", GUILayout.Width(50)))
            {
                ClearSelections();
            }

            EditorGUILayout.EndHorizontal();

            for (int l_index = PreviousSelections.arraySize - 1; l_index >= 0; l_index--)
            {
                Object l_selection = PreviousSelections.ElementAt(l_index).objectReferenceValue;
                DrawSelection(l_selection, false);
            }
        }

        private void DrawSelection(Object p_selection, bool p_isFavorite)
        {
            EditorGUILayout.BeginHorizontal();

            GUIStyle l_iconStyle = new GUIStyle(EditorStyles.label);
            l_iconStyle.alignment = TextAnchor.MiddleCenter;

            Color l_guiColor = GUI.color;
            Color l_baseColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            GUI.color = p_isFavorite ? l_baseColor : l_baseColor * 0.5f;
            if (GUILayout.Button(m_favoriteTexture, EditorStyles.label, GUILayout.Width(20)))
            {
                if (p_isFavorite)
                {
                    UnfavoriteSelection(p_selection);
                }
                else
                {
                    FavoriteSelection(p_selection);
                }
            }

            GUI.color = l_guiColor;
            EditorGUILayout.ObjectField(p_selection, typeof(Object), true);

            EditorGUILayout.EndHorizontal();
        }

        private void OnSelectionChange()
        {
            if (!Selection.activeObject || !AssetDatabase.Contains(Selection.activeObject))
            {
                return;
            }

            AddPreviousSelection(Selection.activeObject);
            Data.ApplyModifiedProperties();

            Repaint();
        }

        private void AddPreviousSelection(Object p_selection)
        {
            PreviousSelections.Remove(p_selection);
            if (PreviousSelections.arraySize >= MaxItems.intValue)
            {
                PreviousSelections.RemoveAt(0);
            }

            PreviousSelections.Add(p_selection);
        }

        private void FavoriteSelection(Object p_selection)
        {
            if (p_selection != null)
            {
                FavoriteSelections.Add(p_selection);
                PreviousSelections.Remove(p_selection);
            }
        }

        private void UnfavoriteSelection(Object p_selection)
        {
            FavoriteSelections.Remove(p_selection);
            if (p_selection != null)
            {
                AddPreviousSelection(p_selection);
            }
        }

        private void ClearSelections()
        {
            PreviousSelections.Clear();
        }

        private void ClearFavoriteSelections()
        {
            FavoriteSelections.Clear();
        }

        private void CleanUpInvalidSelections()
        {
            int l_count = Mathf.Max(PreviousSelections.arraySize, FavoriteSelections.arraySize);
            for (int l_i = 0; l_i < l_count; l_i++)
            {
                if (l_i < PreviousSelections.arraySize && PreviousSelections.ElementAt(l_i).objectReferenceValue == null)
                {
                    PreviousSelections.RemoveAt(l_i);
                }

                if (l_i < FavoriteSelections.arraySize && FavoriteSelections.ElementAt(l_i).objectReferenceValue == null)
                {
                    FavoriteSelections.RemoveAt(l_i);
                }
            }
        }
    }
}