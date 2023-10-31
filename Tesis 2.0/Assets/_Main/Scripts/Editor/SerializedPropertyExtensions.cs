using UnityEditor;
using UnityEngine;

namespace _Main.Scripts.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static bool Contains(this SerializedProperty p_property, Object p_obj)
        {
            for (int l_i = 0; l_i < p_property.arraySize; l_i++)
            {
                if (p_property.GetArrayElementAtIndex(l_i)?.objectReferenceValue == p_obj)
                {
                    return true;
                }
            }

            return false;
        }
    
        public static SerializedProperty Insert(this SerializedProperty p_property, int p_index)
        {
            p_property.InsertArrayElementAtIndex(p_index);
            return p_property.GetArrayElementAtIndex(p_index);
        }

        public static void Add(this SerializedProperty p_property, Object p_obj)
        {
            p_property.Insert(p_property.arraySize).objectReferenceValue = p_obj;
        }
    
        public static bool Remove(this SerializedProperty p_property, Object p_obj)
        {
            for (int l_i = 0; l_i < p_property.arraySize; l_i++)
            {
                if (p_property.GetArrayElementAtIndex(l_i)?.objectReferenceValue == p_obj)
                {
                    p_property.RemoveAt(l_i);
                    return true;
                }
            }

            return false;
        }

        public static SerializedProperty ElementAt(this SerializedProperty p_property, int p_index)
        {
            return p_property.GetArrayElementAtIndex(p_index);
        }
    
        public static void RemoveAt(this SerializedProperty p_property, int p_index)
        {
            p_property.DeleteArrayElementAtIndex(p_index);
        }

        public static void Clear(this SerializedProperty p_property)
        {
            p_property.ClearArray();
            p_property.arraySize = 0;
        }
    }
}
