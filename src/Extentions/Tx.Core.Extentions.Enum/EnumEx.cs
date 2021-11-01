using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Tx.Core.Extentions.Enum
{
    public static class EnumEx
    {
        public static String AsName(this System.Enum @this)
        {
            Type type = @this.GetType();
            return System.Enum.GetName(type, @this);
        }

        public static String AsDescription(this System.Enum @this)
        {
            Type type = @this.GetType();

            string name = System.Enum.GetName(type, @this);

            MemberInfo member = type
                .GetMembers()
                .FirstOrDefault(w => w.Name == name);

            DescriptionAttribute attribute = member != null
                ? member
                    .GetCustomAttributes(true)
                    .FirstOrDefault(w => w.GetType() == typeof(DescriptionAttribute)) as DescriptionAttribute
                : null;

            return attribute != null ? attribute.Description : name;
        }

        public static int AsValue(this System.Enum @this)
        {
            Type type = @this.GetType();
            return (int)System.Enum.Parse(type, @this.ToString());
        }

        public static string ToEnumString<TEnum>(this int enumValue)
        {
            var enumString = enumValue.ToString();
            if (!System.Enum.IsDefined(typeof(TEnum), enumValue)) return enumString;

            enumString = ((TEnum)System.Enum.ToObject(typeof(TEnum), enumValue)).ToString();
            return enumString;
        }

        public static T ToEnum<T>(this string enumString) => (T)System.Enum.Parse(typeof(T), enumString);

        public static IReadOnlyList<T> GetValues<T>() { return (T[])System.Enum.GetValues(typeof(T)); }

        public static string EnumJoin<T>()
        {
            var values = (T[])System.Enum.GetValues(typeof(T));

            return string.Join(",", values.Select(x => x));
        }

        public static bool Has(this System.Enum @this, int value)
        {
            Type type = @this.GetType();
            return System.Enum.IsDefined(type, value);
        }

        public static bool Has(this System.Enum @this, string value)
        {
            Type type = @this.GetType();
            return System.Enum.IsDefined(type, value);
        }

        public static Dictionary<int, string> ToDict(this System.Enum theEnum) => System.Enum.GetValues(theEnum.GetType()).Cast<int>().ToDictionary(enumValue => enumValue, enumValue => enumValue.ToString());
    }
}
