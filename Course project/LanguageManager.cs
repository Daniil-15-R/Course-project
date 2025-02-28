using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_project
{
    public static class LanguageManager
    {
        public static string CurrentLanguage { get; set; } = "Русский"; // Значение по умолчанию

        public static void SetLanguage(string language)
        {
            CurrentLanguage = language;
        }
    }

}
