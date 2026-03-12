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
    /// Логика взаимодействия для PageTasks.xaml
    /// </summary>
    public partial class PageTasks : Page
    {
        public PageTasks()
        {
            InitializeComponent();
            listProducts.ItemsSource = AppData.AppConnect.model01.Recipes.ToList();
            //Fill();
        }


        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listProducts.ItemsSource = FindProduct();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void buttonaddreciep_Click(object sender, RoutedEventArgs e)
        {
            AppData.AppFrame.Framemain.Navigate(new Pages.AddRecipe(null));

        }

        private void buttonback_Click(object sender, RoutedEventArgs e)
        {
            AppData.AppFrame.Framemain.GoBack();
        }

        private void listProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listProducts.SelectedItem is Recipes selectedRecipe)
            {
                NavigationService.Navigate(new AddRecipe(selectedRecipe));
                listProducts.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Выделите рецепт!");
            }

        }
        Recipes[] FindProduct()
        {
            var product = AppConnect.model01.Recipes.ToList();
            var productall = product;
            if (TextSearch != null)
            {
                product = product.Where(x => x.RecipeName.ToLower().Contains(TextSearch.Text.ToLower())).ToList();
            }
            if (ComboFilter.SelectedIndex > 0)
            {
                switch (ComboFilter.SelectedIndex)
                {
                    case 1:
                        product = product.Where(x => x.CookingTime > 0 && x.CookingTime < 10).ToList();
                        break;
                    case 2:
                        product = product.Where(x => x.CookingTime >= 10 && x.CookingTime < 15).ToList();
                        break;
                    case 3:
                        product = product.Where(x => x.CookingTime > 15).ToList();
                        break;
                }
            }
            if (ComboSort.SelectedIndex > 0)
            {
                switch (ComboSort.SelectedIndex)
                {
                    case 1:
                        product = product.OrderBy(x => x.CategoryID).ToList();
                        break;
                    case 2:
                        product = product.OrderByDescending(x => x.CategoryID).ToList();
                        break;
                }
                if (product.Count > 0)
                {
                    TextSearch.Text = "Найдено" + product.Count + "из" + productall.Count;
                }
                else
                {
                    TextSearch.Text = "Ничего не найдено";
                }

            }
            return product.ToArray();
        }
        
    }
}
