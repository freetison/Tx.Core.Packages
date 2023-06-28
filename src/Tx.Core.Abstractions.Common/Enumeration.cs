namespace Tx.Core.Abstractions.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Tx.Core.Abstractions.Common;

    public abstract class Enumeration : IComparable
    {
        public double Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }


        protected Enumeration(double id, string name)
        {
            Id = id;
            Name = name;
        }

        protected Enumeration(double id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public double GetValue() => this.Id;
        public string GetDescription() => this.Description;
        public override string ToString() => this.Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null) { return false; }

            var typeMatches = this.GetType() == obj.GetType();
            var valueMatches = this.Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => this.Id.CompareTo(((Enumeration)other).Id);

        public override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            if (ReferenceEquals(left, null)) { return ReferenceEquals(right, null); }

            return left.Equals(right);
        }

        public static bool operator !=(Enumeration left, Enumeration right) => !(left == right);

        public static bool operator <(Enumeration left, Enumeration right) => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        public static bool operator <=(Enumeration left, Enumeration right) => ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        public static bool operator >(Enumeration left, Enumeration right) => !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        public static bool operator >=(Enumeration left, Enumeration right) => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}

