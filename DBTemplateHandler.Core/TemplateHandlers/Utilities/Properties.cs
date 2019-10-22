using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.TemplateHandlers.Utilities
{
    public class Properties
    {
        private readonly Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public bool ContainsKey(string propertyKey)
        {
            return dictionary.ContainsKey(propertyKey);
        }

        public string getProperty(string propertyKey)
        {
            return dictionary[propertyKey];
        }

        public void setProperty(string propertyKey, string value)
        {
            dictionary[propertyKey] = value;
        }
    }
}
