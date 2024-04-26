using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace _Main.Scripts.Extensions
{
    public static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> p_baseList)
        {
            return p_baseList[Random.Range(0, p_baseList.Count)];
        }
        
        public static List<T> GetUnmatchedElements<T>(ICollection<T> list1, List<T> list2)
        {
            List<T> unmatchedList = list1.Except(list2).ToList();
            unmatchedList.AddRange(list2.Except(list1));

            return unmatchedList;
        }
    }
}