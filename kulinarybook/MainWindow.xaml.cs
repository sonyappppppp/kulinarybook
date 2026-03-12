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

namespace kulinarybook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AppData.AppConnect.model01 = new AppData.kulinarbookEntities2();
            AppData.AppFrame.Framemain = frame1;
            frame1.Navigate(new Pages.Page1());

            InitializeDatabaseConnection();

            // 2. Навигация
            SetupNavigationSystem();

            // 3. 🆕 YANDEXGPT - ОДНА СТРОКА!
            _ = AppConnect.InitYandexGPTAsync();

            // 4. Авторизация
            LoadAuthorizationPage();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }
      

        private void frame1_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
