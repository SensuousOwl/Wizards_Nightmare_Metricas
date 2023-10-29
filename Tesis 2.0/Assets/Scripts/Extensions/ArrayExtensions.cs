using UnityEngine;

namespace _Main._main.Scripts.Extensions
{
    public static class ArrayExtensions
    {
        public static T GetRandomElement<T>(this T[] p_baseArray)
        {
            return p_baseArray[Random.Range(0, p_baseArray.Length)];
        }
    }
}