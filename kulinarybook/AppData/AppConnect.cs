using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace kulinarybook.AppData
{
    public static class AppConnect
    {
        public static kulinarbookEntities3 model01;
        public static int AuthorID;

        public static async Task InitYandexGPTAsync()
        {
            // Загружаем настройки
            YandexGPT.LoadSettings();

            // Проверяем подключение
            if (await YandexGPT.TestConnectionAsync())
            {
                System.Diagnostics.Debug.WriteLine("✅ YandexGPT готов!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("⚠️ YandexGPT не настроен");
            }
        }
    }
    public partial class Recipes
    {
        public string CurrentPhoto
        {
            get
            {
                if (String.IsNullOrEmpty(Image) || String.IsNullOrWhiteSpace(Image))
                    return @"\Images\cookie.png";
                else
                    return @"\Images\" + Image;
            }
        }

    }
}
