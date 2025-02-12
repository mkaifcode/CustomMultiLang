using System.Globalization;
using System.Text.Json;

namespace MultiLangApi.Services.Localization
{
    public class JsonLocalizationService
    {
        private readonly string _resourcesPath = "Resources"; // Folder for JSON files

        public string GetLocalizedString(string key, string? culture = null)
        {
            if (string.IsNullOrEmpty(culture))
            {
                culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }
            else
            {
                culture = culture.Trim('"'); // Removes extra quotes (fixes "\"en\"")
            }

            var filePath = Path.Combine(_resourcesPath, $"messages.{culture}.json");

            if (!File.Exists(filePath))
            {
                return $"[Missing Translation] {key}"; // Returns a debug message if file is missing
            }

            var jsonData = File.ReadAllText(filePath);
            var translations = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData);

            return translations != null && translations.ContainsKey(key) ? translations[key] : $"[Missing Key] {key}";
        }
    }
}
