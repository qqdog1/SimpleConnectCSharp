using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Common.PropertiesReader
{
    public class Properties
    {
        private Dictionary<string, string> dictionary;

        public Properties(String sFilePath)
        {
            dictionary = new Dictionary<string, string>();
            foreach (string line in File.ReadAllLines(sFilePath))
            {
                if ((!string.IsNullOrEmpty(line)) && (!line.StartsWith("#")) && (line.Contains('=')))
                {
                    int index = line.IndexOf('=');
                    string key = line.Substring(0, index).Trim();
                    string value = line.Substring(index + 1).Trim();
                    dictionary.Add(key, value);
                }
            }
        }

        public string getProperty(string sKey)
        {
            if(dictionary.ContainsKey(sKey))
            {
                return dictionary[sKey];
            }
            return null;
        }
    }
}
