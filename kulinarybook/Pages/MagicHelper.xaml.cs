

using kulinarybook.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kulinarybook.Pages
{
    /// <summary>
    /// Логика взаимодействия для MagicHelper.xaml
    /// </summary>
    public partial class MagicHelper : Page
    {
        public MagicHelper()
        {
            InitializeComponent();
            AddMessage(" Привет! Я твой Бри - помощник. Задай вопрос о рецептах!",
false);
        }
        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync();
        }
        private async void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                await SendMessageAsync();
            }
        }
        private async Task SendMessageAsync()
        {
            var message = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            // Показываем сообщение пользователя
            AddMessage($" {message}", true);
            txtInput.Clear();

            // Показываем "печатает..."
            AddMessage("Печатает...", false, true);

            try
            {
                // Отправляем запрос
                var response = await YandexGPT.SendMessageAsync(message);

                // Удаляем "печатает..."
                RemoveTypingIndicator();

                // Показываем ответ
                AddMessage($"🦉 {response}", false);
            }
            catch (Exception ex)
            {
                RemoveTypingIndicator();
                AddMessage($"⚠️ Ошибка: {ex.Message}", false);
            }
        }
        private void AddMessage(string text, bool isUser, bool isTyping = false)
        {
            Dispatcher.Invoke(() =>
            {
                var border = new Border
                {
                    Background = isUser ?
                new SolidColorBrush(Color.FromRgb(80, 8, 5)) :
                new SolidColorBrush(Color.FromRgb(245, 245, 220)),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(10),
                    Margin = new Thickness(isUser ? 50 : 10, 5, isUser ? 10 : 50, 5),
                    HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                    Name = isTyping ? "TypingIndicator" : null
                };

                var textBlock = new TextBlock
                {
                    Text = text,
                    Foreground = isUser ?
                new SolidColorBrush(Colors.Gold) :
                new SolidColorBrush(Color.FromRgb(80, 8, 5)),
                    TextWrapping = TextWrapping.Wrap
                };

                border.Child = textBlock;
                ChatPanel.Children.Add(border);

                // Прокрутка вниз
                ChatScroller.ScrollToBottom();
            });
        }
        private void RemoveTypingIndicator()
        {
            Dispatcher.Invoke(() =>
            {
                for (int i = ChatPanel.Children.Count - 1; i >= 0; i--)
                {
                    if (ChatPanel.Children[i] is Border b && b.Name == "TypingIndicator")
                {
                        ChatPanel.Children.RemoveAt(i);
                        break;
                    }
                }
            });
        }
    }
}

