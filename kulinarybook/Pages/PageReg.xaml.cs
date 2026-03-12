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
using System.Xml.Linq;

namespace kulinarybook.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageReg.xaml
    /// </summary>
    public partial class PageReg : Page
    {
        public PageReg()
        {
            InitializeComponent();
        }

      

        private void Passw1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Passw1.Password != Passw2.Password)
            {
                ButtonRegReg.IsEnabled = false;
                Passw2.Background = Brushes.LightCoral;
                Passw2.BorderBrush = Brushes.Red;
            }
            else
            {
                ButtonRegReg.IsEnabled = true;
                Passw2.Background = Brushes.LightGreen;
                Passw2.BorderBrush = Brushes.Green;
            }
        }




   

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AppData.AppFrame.Framemain.GoBack();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        
            {
                try
                {

                    if (AppConnect.model01.Authors.Count(x => x.Login == TextBoxLogin.Text) > 0)
                    {
                        MessageBox.Show("Пользователь с таким логином есть!", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    Authors userObj = new Authors()
                    {
                        Login = TextBoxLogin.Text,
                        AuthorName = TextBoxAuthorName.Text,
                        Password = Passw1.Password,
                        DateBirth = DateB.SelectedDate.Value,
                        Stash = TextBoxStash.Text,
                        Phone = int.Parse(TextBoxPhone.Text),
                        Email = TextBoxEmail.Text

                    };
                    AppConnect.model01.Authors.Add(userObj);
                    AppConnect.model01.SaveChanges();

                    MessageBox.Show("Данные успешно добавлены!", "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    AppData.AppFrame.Framemain.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            
        }

        private void TextBoxAuthorName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

