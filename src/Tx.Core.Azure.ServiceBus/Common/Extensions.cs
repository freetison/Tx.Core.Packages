using System.Linq;

namespace Tx.Core.Azure.ServiceBus.Common;

public static class Extensions
{
    public static string CleanAsString(this string input) => string.Concat(input.Where(char.IsLetter));
}