namespace Tx.Core.Abstractions.Common
{
    using System.Collections;
    using System.Collections.Generic;

    public class DefaultableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;
        private readonly TValue _defaultValue;

        public int Count => _dictionary.Count;

        public bool IsReadOnly => _dictionary.IsReadOnly;

        public TValue this[TKey key]
        {
            get
            {
                try { return _dictionary[key]; }
                catch (KeyNotFoundException) { return _defaultValue; }
            }

            set => _dictionary[key] = value;
        }

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values
        {
            get { var values = new List<TValue>(_dictionary.Values) { _defaultValue }; return values; }
        }

        public DefaultableDictionary(IDictionary<TKey, TValue> dictionary, TValue defaultValue) => (_dictionary, _defaultValue) = (dictionary, defaultValue);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item) => _dictionary.Add(item);

        public void Clear() => _dictionary.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) => _dictionary.Remove(item);

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public void Add(TKey key, TValue value) => _dictionary.Add(key, value);

        public bool Remove(TKey key) => _dictionary.Remove(key);

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!_dictionary.TryGetValue(key, out value)) { value = _defaultValue; }

            return true;
        }

    }
}
