using Stratums.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.HelperMethods
{
    public static class ListExtension
    {
        public static List<T> ToSingleList<T>(this List<List<T>> initial)
        {
            List<T> simplified = new List<T>();

            foreach (var outerType in initial)
            foreach (var innerType in outerType)
            {
                if (!simplified.Contains(innerType))
                {
                    simplified.Add(innerType);
                }
            }

            return simplified;
        }

        public static List<T> AddWithoutRepeats<T>(this List<T> current, List<T> adding)
        {
            foreach (var item in adding) 
            {
                if (!current.Contains(item))
                {
                    current.Add(item);
                }
            }

            return current;
        }

        public static List<T> AddWithoutRepeats<T>(this List<T> current, T adding)
        {
            if (!current.Contains(adding))
            {
                current.Add(adding);
            }

            return current;
        }
        public static Dictionary<Key, Value> AddWithoutRepeats<Key, Value>(this Dictionary<Key, Value> current, Dictionary<Key, Value> adding)
        {
            foreach (var item in adding)
            {
                if (!current.Contains(item))
                {
                    current.Add(item.Key, item.Value);
                }
            }

            return current;
        }

        public static Dictionary<Key, Value> AddWithoutRepeats<Key, Value>(this Dictionary<Key, Value> current, KeyValuePair<Key, Value> adding)
        {
            if (!current.Contains(adding))
            {
                current.Add(adding.Key, adding.Value);
            }

            return current;
        }
    }
}
