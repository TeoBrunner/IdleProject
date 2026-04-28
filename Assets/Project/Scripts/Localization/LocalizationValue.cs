using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public class LocalizationValue
    {
        private readonly Dictionary<string, string> values;

        public LocalizationValue(Dictionary<string, string> values)
        {
            this.values = values;
        }

        public string Get(string language)
        {
            if (values.ContainsKey(language))
            {
                return values[language];
            }
            if (values.ContainsKey("EN"))
            {
                return values["EN"];
            }
            //if(values.ContainsKey(LocalizationManager.DefaultLanguage))
            //{
            //    return values[LocalizationManager.DefaultLanguage];
            //}
            return null;
        }
    }
}

