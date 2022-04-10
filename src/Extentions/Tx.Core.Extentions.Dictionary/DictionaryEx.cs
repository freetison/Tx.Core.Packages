using System.Collections.Generic;
using System.Linq;

namespace Tx.Core.Extensions.Dictionary
{
    public static class DictionaryEx
    {
        public static TKey GetKeyFromValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value) =>
            dictionary.FirstOrDefault(x => x.Value.Equals(value)).Key;

        public static TValue GetValueFromKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
            dictionary.FirstOrDefault(x => x.Key.Equals(key)).Value;

        public static bool CheckDictionaryIsNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) => (dictionary == null);

        public static bool AddIfNotExists<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.CheckDictionaryIsNull() && dictionary.ContainsKey(key)) { return false; }

            dictionary.Add(key, value);
            return false;
        }

        public static bool DeleteIfExistsKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (!dictionary.CheckDictionaryIsNull() && !dictionary.ContainsKey(key)) { return false; }
            return dictionary.Remove(key);
        }

        public static bool Update<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.CheckDictionaryIsNull() && CheckKeyValuePairIsNull(key, value)) { return false; }
            dictionary[key] = value;
            return true;
        }

        private static bool CheckKeyValuePairIsNull<TKey, TValue>(TKey key, TValue value) => (key == null || value == null);

        private static bool CheckKeyValuePairIsNull<TKey, TValue>(KeyValuePair<TKey, TValue> pair) => (pair.Key == null || pair.Value == null);
    }
}
