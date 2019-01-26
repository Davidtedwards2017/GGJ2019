using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Collections;
namespace Utilites
{
    public static class GenericExtensions
    {
        public static bool Exists(this string value)
        {
            return (!string.IsNullOrEmpty(value));
        }

        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static bool Is<T>(this object obj) where T : Component
        {
            if (obj == null) return false;

            if (obj is GameObject)
            {
                GameObject go = (GameObject)obj;
                var component = go.GetComponent<T>();
                return component != null;
            }
            else if (obj is T)
            {
                return true;
            }

            return false;
        }

        public static T Get<T>(this object obj) where T : Component
        {
            if (obj == null) return null;

            if (obj is GameObject)
            {
                GameObject go = (GameObject)obj;
                var component = go.GetComponent<T>();
                return component;
            }
            else if (obj is T)
            {
                return (T)obj;
            }

            return null;
        }

        public static T[] Shift<T>(this T[] collection)
        {
            T[] temp = new T[collection.Length];
            for (int i = 0; i < collection.Length - 1; i++)
            {
                temp[i + 1] = collection[i];
            }

            return temp;
        }

        public static void AddIfUnique<T>(this List<T> collection, T entry)
        {
            if (entry != null && !collection.Contains(entry))
            {
                collection.Add(entry);
            }
        }

        public static void AddIfUnique<T>(this List<T> collection, IEnumerable<T> entries)
        {
            foreach (var entry in entries)
            {
                collection.AddIfUnique(entry);
            }
        }


        public static void SafeAdd<T>(this List<T> collection, T value)
        {
            if (!collection.Contains(value))
            {
                collection.Add(value);
            }
        }

        public static void SafeAdd<T, K>(this List<KeyValuePair<T, K>> collection, T key, K value)
        {
            foreach (var pair in collection)
            {
                if (pair.Key.Equals(key) && pair.Value.Equals(value))
                {
                    return;
                }
            }

            collection.Add(new KeyValuePair<T, K>(key, value));
        }

        public static T SafeGet<T>(this Dictionary<string, T> dictionary, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (dictionary.ContainsKey(key))
                {
                    return dictionary[key];
                }
            }

            return default(T);
        }

        public static void SafeRemove<T>(this List<T> collection, T value)
        {
            if (collection.Contains(value))
            {
                collection.Remove(value);
            }
        }

        public static void SafeRemove<T, K>(this List<KeyValuePair<T, K>> collection, T key, K value)
        {
            foreach (var pair in collection)
            {
                if (pair.Key.Equals(key) && pair.Value.Equals(value))
                {
                    collection.Remove(pair);
                    return;
                }
            }
        }

        public static List<T> RemoveMatchingEntries<T>(this List<T> collection, Predicate<T> matching)
        {
            var temp = new List<T>(collection);
            foreach (var entry in collection)
            {
                if (matching(entry))
                {
                    temp.Remove(entry);
                }
            }

            return temp;
        }

        public static void SafeRemove<T, K>(this Dictionary<T, K> dictionary, T key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }
        }

        public static K SafeGetOrInitialize<T, K>(this Dictionary<T, K> dictionary, T key) where K : new()
        {
            if (dictionary.ContainsKey(key))
            {
                return (K)dictionary[key];
            }
            else
            {
                var instance = new K();
                dictionary.Add(key, instance);
                return instance;
            }
        }

        public static void SafeSet<T, K>(this Dictionary<T, K> dictionary, T key, K value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void AddOrUpdate<T, K>(this Dictionary<T, K> dictionary, T key, K value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static T[] FlipRows<T>(this T[] grid, int numRows, int numColumns)
        {
            var tempMatrix = new T[grid.Length];
            int index = 0;
            for (int r = numRows - 1; r >= 0; r--)
            {
                for (int c = 0; c < numColumns; c++)
                {
                    tempMatrix[index++] = grid[(r * numColumns) + c];
                }
            }

            return tempMatrix;
        }

        public static T[] FlipColumns<T>(this T[] grid, int numRows, int numColumns)
        {
            var tempMatrix = new T[grid.Length];
            int index = 0;
            for (int r = 0; r < numRows; r++)
            {
                for (int c = numColumns - 1; c >= 0; c--)
                {
                    tempMatrix[index++] = grid[(r * numColumns) + c];
                }
            }

            return tempMatrix;
        }

        public static T[] SwapRowsAndColumns<T>(this T[] grid, ref int numRows, ref int numColumns)
        {
            int newRows = numColumns;
            int newCols = numRows;

            numRows = newRows;
            numColumns = newCols;

            var tempMatrix = new T[grid.Length];
            int index = 0;
            for (int r = 0; r < numRows; r++)
            {
                for (int c = numColumns - 1; c >= 0; c--)
                {
                    tempMatrix[index++] = grid[(c * numRows) + r];
                }
            }

            return tempMatrix;
        }

        public static T PickRandom<T>(this List<T> collection)
        {
            if (!collection.Any())
            {
                return default(T);
            }

            //UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);
            return collection[UnityEngine.Random.Range(0, collection.Count)];
        }

        public static List<T> PickRandom<T>(this List<T> collection, int count)
        {
            if (!collection.Any())
            {
                return null;
            }

            var pickedCollection = new List<T>();
            var pool = new List<T>(collection);

            while (count-- > 0 && pool.Any())
            {
                var picked = pool.PickRandom();
                pool.Remove(picked);

                pickedCollection.Add(picked);
            }

            return pickedCollection;
        }

        public static T RemoveRandom<T>(this List<T> collection)
        {
            var instance = collection.PickRandom();
            collection.Remove(instance);
            return instance;
        }

        public static int[] Shift(this int[] myArray)
        {
            int[] tArray = new int[myArray.Length];
            for (int i = 0; i < myArray.Length; i++)
            {
                if (i < myArray.Length - 1)
                    tArray[i] = myArray[i + 1];
                else
                    tArray[i] = myArray[0];
            }
            return tArray;
        }

        public static GameObject RootTo(this GameObject gameObject, GameObject parent, Vector3 offset)
        {
            return gameObject.AttachTo(parent).SetLocalPosition(offset);
        }

        public static GameObject AttachTo(this Component component, Component parent)
        {
            return component.gameObject.AttachTo(parent.gameObject);
        }

        public static GameObject AttachTo(this GameObject gameObject, GameObject parent)
        {
            if (gameObject == null) return null;

            if (parent == null)
            {
                gameObject.transform.SetParent(null);
            }
            else
            {
                gameObject.transform.SetParent(parent.transform);
            }

            return gameObject;
        }

        public static GameObject SetLocalPosition(this GameObject gameObject, Vector3 position)
        {
            if (gameObject != null)
            {
                gameObject.transform.localPosition = position;
            }

            return gameObject;
        }

    }
}