namespace Tx.Core.Abstractions.Common.Interfaces
{
    using System.Collections.Generic;
    using Abstractions.Common;
using Tx.Core.Abstractions.Common;

    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}
