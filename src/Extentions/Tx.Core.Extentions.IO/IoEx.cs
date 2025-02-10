using System;
using System.IO;
using System.Text.RegularExpressions;

using Tx.Core.Extensions.String;

namespace Tx.Core.Extentions.IO
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

        public static void AppendText(string path, string text)
        {
            if (File.Exists(path))
            {
                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine(text);
                    writer.Close();
                }
                return;
            }
            File.WriteAllText(path, text);
            using (StreamWriter writer = File.AppendText(path))
            {
                writer.Close();
            }
        }

        public static bool CopyFolder(string sourcePath, string targetPath)
        {
            string fileName = "";
            string destFile = "";
            if (!Directory.Exists(targetPath))
            {
                DirectoryInfo directoryInfo1 = Directory.CreateDirectory(targetPath);
            }
            if (Directory.Exists(sourcePath))
            {
                foreach (string s in Directory.GetFiles(sourcePath))
                {
                    fileName = Path.GetFileName(s);
                    destFile = Path.Combine(targetPath, fileName);
                    File.Copy(s, destFile, true);
                }
                return true;
            }
            else
                return false;
        }

        public static bool DeleteFolder(string dir, bool recursivo)
        {
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, recursivo);
                return true;
            }
            else
                return false;
            //Console.WriteLine("Source path does not exist!");
        }

        public static int CountStr(string file, string str)
        {
            string text;
            MatchCollection tokensCollection;
            int i1;
            using (TextReader reader = ((TextReader)File.OpenText(file)))
            {
                text = reader.ReadToEnd();
                reader.Close();
                tokensCollection = Regex.Matches(text, str);
                i1 = tokensCollection.Count;
            }
            return i1;
        }
    }
}