using System;
using System.Collections.Generic;
using System.Linq;

namespace Tx.Core.GenericFactory
{
    /// Use:
    /// var genericFactory = new FullGenericFactory<int, IVehicle>();
    /// genericFactory.Register(1, () => new Car(new Person("Roland")));
    /// genericFactory.Register(2, () => new Motorcycle(new Person("Isa")));
    /// var result = genericFactory.Get(2);
    public class GenericFactory<T, TK> where TK : class
    {
        private Dictionary<T, Func<TK>> _dict = new Dictionary<T, Func<TK>>();

        public GenericFactory() { }

        public void Register(T type, Func<TK> ctor)
        {
            if (ctor is null) return;
            _dict.Add(type, ctor);
        }

        public TK Get(T type) => _dict.TryGetValue(type, out var constructor) ? constructor() : default(TK);

        public IList<Func<TK>> GetAll() => _dict.Count > 0 ? _dict.Values.ToList() : null;
    }
}
