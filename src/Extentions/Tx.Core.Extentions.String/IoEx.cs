using System;
using System.IO;

namespace Tx.Core.Extensions.String
{
    public static class IoEx
    {
        public static T LoadJson<T>(string jsonPath)
        {
            try
            {
                using StreamReader streamReader = new StreamReader(jsonPath);
                return streamReader.ReadToEnd().ParseTo<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }
    }
}
