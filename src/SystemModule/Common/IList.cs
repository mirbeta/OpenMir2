using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameFramework
{
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    [ComVisible(false)]
    public class IList<TKey, TValue>
    {
        public int Count
        {
            get { return dictionary.Count; }
        }

        private readonly Dictionary<TKey, TValue> dictionary;

        public IList()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        public TValue this[TKey index]
        {
            get
            {
                if (!dictionary.ContainsKey(index))
                {
                    throw new Exception("字典中没有给定的索引");
                }
                return dictionary[index];
            }
            set
            {
                dictionary[index] = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
        }

        public void RemoveAt(TKey key)
        {
            dictionary.Remove(key);
        }

        public void Clear()
        {
            dictionary.Clear();
        }
    }
}