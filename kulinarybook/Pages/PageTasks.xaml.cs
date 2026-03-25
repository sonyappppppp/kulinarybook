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
using System.Data.Entity;

namespace kulinarybook.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageTasks.xaml
    /// </summary>
    public partial class PageTasks : Page
    {
        private kulinarbookEntities3 db;
        private List<Recipes> allRecipes;
        public PageTasks()
        {
            InitializeComponent();
            db = new kulinarbookEntities3();
           listProducts.ItemsSource = AppData.AppConnect.model01.Recipes.ToList();
            LoadData();
            //Fill();
        }

        private void LoadData()
        {
            try
            {
                // Загружаем данные в поле класса allRecipes
                allRecipes = db.Recipes
                    .Include(r => r.Categories)
                    .Include(r => r.Authors)
                    .ToList();

                // Загрузка категорий для ComboBox
                var categoriesList = db.Categories.ToList();
                ComboFilter.Items.Add("Все категории");
                foreach (var category in categoriesList)
                {
                    ComboFilter.Items.Add(category.CategoryName);
                }
                ComboFilter.SelectedIndex = 0;

                // Загрузка опций сортировки
                ComboSort.Items.Add("По названию (А-Я)");
                ComboSort.Items.Add("По названию (Я-А)");
                ComboSort.Items.Add("По времени приготовления (возрастание)");
                ComboSort.Items.Add("По времени приготовления (убывание)");
                ComboSort.Items.Add("По автору (А-Я)");
                ComboSort.Items.Add("По автору (Я-А)");
                ComboSort.SelectedIndex = 0;

                ApplyFilterAndSort();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilterAndSort()
        {
            if (allRecipes == null) return;

            var filteredRecipes = allRecipes.ToList();

            // Фильтрация по категории
            if (ComboFilter.SelectedItem != null && ComboFilter.SelectedItem.ToString() != "Все категории")
            {
                string selectedCategory = ComboFilter.SelectedItem.ToString();
                filteredRecipes = filteredRecipes
                    .Where(r => r.Categories != null && r.Categories.CategoryName == selectedCategory)
                    .ToList();
            }

            // Поиск по названию
            if (!string.IsNullOrWhiteSpace(TextSearch.Text))
            {
                string searchText = TextSearch.Text.ToLower();
                filteredRecipes = filteredRecipes
                    .Where(r => r.RecipeName != null && r.RecipeName.ToLower().Contains(searchText))
                    .ToList();
            }

            // Сортировка
            if (ComboSort.SelectedItem != null)
            {
                string sortOption = ComboSort.SelectedItem.ToString();

                switch (sortOption)
                {
                    case "По названию (А-Я)":
                        filteredRecipes = filteredRecipes.OrderBy(r => r.RecipeName).ToList();
                        break;
                    case "По названию (Я-А)":
                        filteredRecipes = filteredRecipes.OrderByDescending(r => r.RecipeName).ToList();
                        break;
                    case "По времени приготовления (возрастание)":
                        filteredRecipes = filteredRecipes.OrderBy(r => r.CookingTime).ToList();
                        break;
                    case "По времени приготовления (убывание)":
                        filteredRecipes = filteredRecipes.OrderByDescending(r => r.CookingTime).ToList();
                        break;
                    case "По автору (А-Я)":
                        filteredRecipes = filteredRecipes
                            .OrderBy(r => r.Authors != null ? r.Authors.AuthorName : "")
                            .ToList();
                        break;
                    case "По автору (Я-А)":
                        filteredRecipes = filteredRecipes
                            .OrderByDescending(r => r.Authors != null ? r.Authors.AuthorName : "")
                            .ToList();
                        break;
                }
            }

            listProducts.ItemsSource = filteredRecipes;
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilterAndSort();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilterAndSort();
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilterAndSort();
        }

        private void listProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRecipe = listProducts.SelectedItem as Recipes;
            if (selectedRecipe != null)
            {
                MessageBox.Show($"Выбран рецепт: {selectedRecipe.RecipeName}", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void buttonaddreciep_Click(object sender, RoutedEventArgs e)
        {
            Recipes newRecipe = new Recipes();
            NavigationService?.Navigate(new AddRecipe(newRecipe));
        }

        private void buttonback_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                Application.Current.MainWindow.Close();
            }
        }
    }
}
