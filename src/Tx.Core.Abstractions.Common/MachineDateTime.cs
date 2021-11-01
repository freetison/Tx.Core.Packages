namespace Tx.Core.Abstractions.Common
{
    using System;
    using Tx.Core.Abstractions.Common.Interfaces;

    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;

        public int CurrentMonth => DateTime.Now.Month;

        public int CurrentDay => DateTime.Now.Day;
    }
}
