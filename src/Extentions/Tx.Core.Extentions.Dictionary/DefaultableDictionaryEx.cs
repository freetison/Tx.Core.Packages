using System.Collections.Generic;

namespace Tx.Core.Extensions.Dictionary
{
    public static class DefaultableDictionaryEx
    {
        public static IDictionary<TKey, TValue> WithDefaultValue<TValue, TKey>(this IDictionary<TKey, TValue> dictionary, TValue defaultValue) => new DefaultableDictionary<TKey, TValue>(dictionary, defaultValue);
    }
}
