using Tx.Core.Entity.Interfaces.Entity;

namespace Tx.Core.Entity.Models.Entity
{
    public abstract class EntityBase<T>: IEntityBase<T>
    {
        private int? _requestedHashCode;

        public virtual T Id { get; protected set; }

        public bool IsTransient() => Id.Equals(default(T));

        public override bool Equals(object obj)
        {
            if (!(obj is EntityBase<T>) || GetType() != obj.GetType()) return false;

            var item = (EntityBase<T>)obj;
            if (item.IsTransient() || IsTransient()) return false;

            return ReferenceEquals(item, this);
        }

        public override int GetHashCode()
        {
            if (IsTransient()) return base.GetHashCode();

            if (!_requestedHashCode.HasValue) { _requestedHashCode = Id.GetHashCode() ^ 31; }
            return _requestedHashCode.Value;
        }

        public static bool operator ==(EntityBase<T> left, EntityBase<T> right)
        {
            if (Equals(left, null)) return Equals(right, null) ? true : false;
            return left.Equals(right);
        }

        public static bool operator !=(EntityBase<T> left, EntityBase<T> right) => !(left == right);

    }
}