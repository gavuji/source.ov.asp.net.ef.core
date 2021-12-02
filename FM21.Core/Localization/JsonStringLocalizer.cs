using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FM21.Core.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly Dictionary<string, string> distResources;

        public JsonStringLocalizer()
        {
            string localizeLanguage = ApplicationConstants.RequestLanguage;
            if (string.IsNullOrEmpty(localizeLanguage))
            {
                localizeLanguage = CultureInfo.CurrentCulture.Name;
            }
            distResources = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@"Resources/" + localizeLanguage + ".json"));
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return distResources.Select(l => new LocalizedString(l.Key, l.Value, true));
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new JsonStringLocalizer();
        }

        private string GetString(string name)
        {
            var value = distResources.FirstOrDefault(l => l.Key == name).Value;
            return Convert.ToString(value);
        }
    }
}