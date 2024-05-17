using UnityEngine;

namespace _Main.Scripts.StaticClass
{
    public static class Logger
    {
        public static void Log(object p_message)
        {
#if UNITY_EDITOR
            Debug.Log(p_message);
#endif
        }

        public static void Log(object p_message, Object p_context)
        {
#if UNITY_EDITOR
            Debug.Log(p_message, p_context);
#endif
        }

        public static void LogWarning(object p_message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(p_message);
#endif
        }

        public static void LogWarning(object p_message, Object p_context)
        {
#if UNITY_EDITOR
            Debug.LogWarning(p_message, p_context);
#endif
        }

        public static void LogError(object p_message)
        {
#if UNITY_EDITOR
            Debug.LogError(p_message);
#endif
        }

        public static void LogError(object p_message, Object p_context)
        {
#if UNITY_EDITOR
            Debug.LogError(p_message, p_context);
#endif
        }
    }
}