using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tx.Core.Extentions.NullPattern
{
    public static class NullPatternEx
    {
        public static T Create<T>() where T : new() => new T();

        public static T NothingIfNull<T>(this T obj) where T : new() => obj == null ? new T() : obj;
    }
}
