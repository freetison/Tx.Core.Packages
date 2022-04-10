using System.Linq;

namespace Tx.Core.Azure.ServiceBus.Common
{
    public enum MessageProcessResponse
    {
        Complete,
        Abandon,
        Dead
    }

    public static class Extentions
    {
        public static string CleanAsString(this string input) => string.Concat(input.Where(char.IsLetter));
    }
}
