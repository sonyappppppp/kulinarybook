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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = AppData.AppConnect.model01.Authors.FirstOrDefault(x =>
                x.Login == textboxlogin.Text && x.Password == password.Password);
                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя нет", "Ошибка авторизации",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Здравствуйте, Автор" + userObj.AuthorName + "!", "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Information); NavigationService.Navigate(new Pages.PageTasks());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString(), "Критическая ошибка приложения",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //private void PageReg_Click(object sender, RoutedEventArgs e)
        //{ AppData.AppFrame.Framemain.Navigate(new Pages.PageReg()); }




        private void Buttonreg_Click(object sender, RoutedEventArgs e)
        {
            AppData.AppFrame.Framemain.Navigate(new Pages.PageReg());
        }

        private void textboxlogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
