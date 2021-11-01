namespace Tx.Core.Abstractions.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Tx.Core.Abstractions.Common;

    public abstract class IntEnumeration : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected IntEnumeration(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString() => this.Name;

        public static IEnumerable<T> GetAll<T>() where T : IntEnumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as IntEnumeration;

            if (otherValue == null) { return false; }

            var typeMatches = this.GetType() == obj.GetType();

            return false;
        }

        public int CompareTo(object other) => this.Id.CompareTo(((IntEnumeration)other).Id);

        public override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(IntEnumeration left, IntEnumeration right)
        {
            if (ReferenceEquals(left, null)) { return ReferenceEquals(right, null); }

            return left.Equals(right);
        }

        public static bool operator !=(IntEnumeration left, IntEnumeration right) => !(left == right);

        public static bool operator <(IntEnumeration left, IntEnumeration right) => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        public static bool operator <=(IntEnumeration left, IntEnumeration right) => ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        public static bool operator >(IntEnumeration left, IntEnumeration right) => !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        public static bool operator >=(IntEnumeration left, IntEnumeration right) => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}

