using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace kulinarybook.AppData
{
    public static class YandexGPT
    {
        public static string FolderId { get; set; } = "b1gaafeovsb8mus2rc51";
        public static string ApiKey { get; set; } = "aje5po2l5jsgritqavdf";
        private static string SystemPrompt =
     "Ты — Поварёнок-Патронус, магический кулинар из Хогвартса. " +
     "Отвечай на русском, коротко, с эмодзи. Помогай с рецептами.";
        // 📊 Статистика
        public static int TotalRequests { get; private set; } = 0;
        public static DateTime LastRequestTime
        {
            get; private set;
        }
        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await SendMessageAsync("Привет! Напиши 'OK'");
                return response.Contains("OK") || !string.IsNullOrEmpty(response);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 💬 ОТПРАВКА СООБЩЕНИЯ - САМЫЙ ВАЖНЫЙ МЕТОД!
        /// </summary>
        public static async Task<string> SendMessageAsync(string userMessage)
        {
            // ❗ Проверка настроек
            if (string.IsNullOrWhiteSpace(FolderId) || string.IsNullOrWhiteSpace(ApiKey))
            {
                throw new Exception("❌ Не указан FolderId или ApiKey в файле YandexGPT.cs!");
            }
            try
            {
                TotalRequests++;
                LastRequestTime = DateTime.Now;
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(30);

                // 📦 Формируем запрос
                var request = new
                {
                    modelUri = $"gpt://{FolderId}/yandexgpt/latest",
                    completionOptions = new
                    {
                        stream = false,
                        temperature = 0.6,
                        maxTokens = 1000
                    },
                    messages = new[]
                {
 new { role = "system", text = SystemPrompt },
 new { role = "user", text = userMessage }
 }
                };
                // 📤 Отправляем
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Api-Key {ApiKey}"
               );

                var response = await client.PostAsync(
                "https://llm.api.cloud.yandex.net/foundationModels/v1/completion"
               ,
                content);

                var responseJson = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Ошибка API: {response.StatusCode}");
                }
                // 📥 Парсим ответ
                var doc = JsonDocument.Parse(responseJson);
                var result = doc.RootElement
                .GetProperty("result")
                .GetProperty("alternatives")[0]
                .GetProperty("message")
                .GetProperty("text")
                .GetString();
                return result ?? "...";
            }
            catch (Exception ex)
            {
                throw new Exception($"🧙 Ошибка YandexGPT: {ex.Message}");
            }
        }
        // ==========================================
        // 🍲 МЕТОДЫ ДЛЯ КУЛИНАРНОЙ КНИГИ
        // ==========================================
        /// <summary>
        /// 🍲 ГЕНЕРАЦИЯ РЕЦЕПТА
        /// </summary>
        public static async Task<string> GenerateRecipeAsync(string recipeName)
        {
            return await SendMessageAsync(
            $"Придумай рецепт из мира Гарри Поттера '{recipeName}'. " +
            $"Коротко: ингредиенты и шаги. С эмодзи.");
        }
        /// <summary>
        /// 📝 УЛУЧШЕНИЕ ОПИСАНИЯ
        /// </summary>
        public static async Task<string> ImproveDescriptionAsync(string description)
        {
            return await SendMessageAsync(
            $"Улучши описание блюда, сделай аппетитным: \"{description}\"");
        }
        /// <summary>
        /// 🔄 ЗАМЕНА ИНГРЕДИЕНТА
        /// </summary>
        public static async Task<string> SubstituteIngredientAsync(string ingredient)
        {
            return await SendMessageAsync(
            $"Чем заменить '{ingredient}' в магической кулинарии? 2 варианта.");
        }
        /// <summary>
        /// 💾 СОХРАНЕНИЕ НАСТРОЕК
        /// </summary>
        public static void SaveSettings()
        {
            var settings = new
            {
                FolderId,
                ApiKey
            };

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText("yandex_config.json", json);
        }
        /// <summary>
        /// 📂 ЗАГРУЗКА НАСТРОЕК
        /// </summary>
        public static void LoadSettings()
        {
            try
            {
                if (File.Exists("yandex_config.json"))
                {
                    var json = File.ReadAllText("yandex_config.json");
                    var doc = JsonDocument.Parse(json);

                    if (doc.RootElement.TryGetProperty("FolderId", out var fId))
                        FolderId = fId.GetString() ?? "";

                    if (doc.RootElement.TryGetProperty("ApiKey", out var aKey))
                        ApiKey = aKey.GetString() ?? "";
                }
                else
                {
                    // Создаём файл-образец
                    SaveSettings();
                }
            }
            catch { /* Игнорим ошибки загрузки */ }
        }
    }
    /// <summary>
    /// 🧙‍♂️ РАСШИРЕНИЯ ДЛЯ УДОБСТВА
    /// </summary>
    public static class YandexGPTExtensions
    {
        public static async Task<string> AskYandexGPT(this string question)
        {
            return await YandexGPT.SendMessageAsync(question);
        }
    }
}


