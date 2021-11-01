using System;
using System.Collections.Generic;
using System.Linq;

namespace Tx.Core.GenericFactory
{
    /// Use:
    /// public enum VehicleType { Car, Truck, Motorcycle };
    /// GenericEnumFactory<IVehicle>.Register(VehicleType.Car, () => new Car(new Person("Roland")));
    /// GenericEnumFactory<IVehicle>.Register(VehicleType.Motorcycle, () => new Motorcycle(new Person("Isa")));
    /// var cls = GenericEnumFactory<IVehicle>.Get(VehicleType.Car);
    public class GenericEnumFactory<T> where T : class
    {
        private Dictionary<Enum, Func<T>> _dict = new Dictionary<Enum, Func<T>>();

        public GenericEnumFactory() { }

        public void Register(Enum type, Func<T> ctor)
        {
            if (ctor is null) return;
            _dict.Add(type, ctor);
        }

        public T Get(Enum type)
        {
            Func<T> constructor = null;
            return _dict.TryGetValue(type, out constructor) ? constructor() : default(T);
        }

        public IList<Func<T>> GetAll() => _dict.Count > 0 ? _dict.Values.ToList() : null;
    }
}
