using kulinarybook.AppData;
using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace kulinarybook.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddRecipe.xaml
    /// </summary>
    public partial class AddRecipe : Page
    {
        public Recipes recipes = new Recipes();
        private string selectedImagePath = null;

        public AddRecipe(Recipes recipe)
        {
            InitializeComponent();
            Fill();

            if (recipe != null)
            {
                recipes = recipe;
                // Если есть изображение, сохраняем путь
                if (!string.IsNullOrEmpty(recipes.Image))
                {
                    selectedImagePath = recipes.Image;
                    imageu.Text = recipes.Image;
                }
            }

            DataContext = recipes;
        }

        public void Fill()
        {
            try
            {
                // Заполнение категорий
                var category = AppConnect.model01.Categories.ToList();
                categorycc.Items.Clear();
                categorycc.Items.Add("Выберите категорию");
                foreach (var item in category)
                {
                    categorycc.Items.Add(item.CategoryName);
                }

                // Если есть выбранная категория, устанавливаем её
                if (recipes.CategoryID.HasValue && recipes.CategoryID.Value > 0)
                {
                    var selectedCategory = category.FirstOrDefault(c => c.CategoryID == recipes.CategoryID.Value);
                    if (selectedCategory != null)
                    {
                        categorycc.SelectedItem = selectedCategory.CategoryName;
                    }
                    else
                    {
                        categorycc.SelectedIndex = 0;
                    }
                }
                else
                {
                    categorycc.SelectedIndex = 0;
                }

                // Заполнение авторов
                var authors = AppConnect.model01.Authors.ToList();
                authorcc.Items.Clear();
                authorcc.Items.Add("Выберите автора");
                foreach (var item in authors)
                {
                    authorcc.Items.Add(item.AuthorName);
                }

                // Если есть выбранный автор, устанавливаем его
                if (recipes.AuthorID.HasValue && recipes.AuthorID.Value > 0)
                {
                    var selectedAuthor = authors.FirstOrDefault(a => a.AuthorID == recipes.AuthorID.Value);
                    if (selectedAuthor != null)
                    {
                        authorcc.SelectedItem = selectedAuthor.AuthorName;
                    }
                    else
                    {
                        authorcc.SelectedIndex = 0;
                    }
                }
                else
                {
                    authorcc.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void namereciep_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Можно добавить валидацию при вводе, если нужно
        }

        private void downloadimage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем диалог выбора файла
                OpenFileDialog openFileDialog = new OpenFileDialog();

                // Настройки диалога
                openFileDialog.Title = "Выберите изображение для рецепта";
                openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Все файлы|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                // Показываем диалог и проверяем, выбрал ли пользователь файл
                if (openFileDialog.ShowDialog() == true)
                {
                    // Получаем путь к выбранному файлу
                    string sourceFilePath = openFileDialog.FileName;

                    // Создаем имя файла для сохранения (уникальное)
                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(sourceFilePath);

                    // Путь для сохранения в папке проекта
                    string projectImagesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                    // Создаем папку Images, если ее нет
                    if (!System.IO.Directory.Exists(projectImagesPath))
                    {
                        System.IO.Directory.CreateDirectory(projectImagesPath);
                    }

                    // Полный путь для сохранения
                    string destinationFilePath = System.IO.Path.Combine(projectImagesPath, fileName);

                    // Копируем файл
                    System.IO.File.Copy(sourceFilePath, destinationFilePath, true);

                    // Сохраняем относительный путь в базу данных
                    selectedImagePath = $"Images/{fileName}";
                    recipes.Image = selectedImagePath;

                    // Обновляем текстовое поле с путем к изображению
                    imageu.Text = selectedImagePath;

                    MessageBox.Show("Изображение успешно загружено!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void savereciep_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            // Проверка названия рецепта
            if (string.IsNullOrWhiteSpace(namereciep.Text))
            {
                sb.Append("Введите название рецепта!\n");
            }
            else
            {
                recipes.RecipeName = namereciep.Text;
            }

            // Проверка описания
            if (string.IsNullOrWhiteSpace(descripreciep.Text))
            {
                sb.Append("Введите описание рецепта!\n");
            }
            else
            {
                recipes.Description = descripreciep.Text;
            }

            // Проверка выбора категории
            if (categorycc.SelectedItem == null || categorycc.SelectedIndex == 0)
            {
                sb.Append("Выберите категорию!\n");
            }
            else
            {
                try
                {
                    var selectedCategory = AppConnect.model01.Categories
                        .FirstOrDefault(x => x.CategoryName == categorycc.SelectedItem.ToString());
                    if (selectedCategory != null)
                    {
                        recipes.CategoryID = selectedCategory.CategoryID;
                    }
                    else
                    {
                        sb.Append("Выбранная категория не найдена!\n");
                    }
                }
                catch (Exception ex)
                {
                    sb.Append($"Ошибка при выборе категории: {ex.Message}\n");
                }
            }

            // Проверка выбора автора
            if (authorcc.SelectedItem == null || authorcc.SelectedIndex == 0)
            {
                sb.Append("Выберите автора!\n");
            }
            else
            {
                try
                {
                    var selectedAuthor = AppConnect.model01.Authors
                        .FirstOrDefault(x => x.AuthorName == authorcc.SelectedItem.ToString());
                    if (selectedAuthor != null)
                    {
                        recipes.AuthorID = selectedAuthor.AuthorID;
                    }
                    else
                    {
                        sb.Append("Выбранный автор не найден!\n");
                    }
                }
                catch (Exception ex)
                {
                    sb.Append($"Ошибка при выборе автора: {ex.Message}\n");
                }
            }

            // Проверка времени приготовления
            if (string.IsNullOrWhiteSpace(timeprigot.Text))
            {
                sb.Append("Введите время приготовления!\n");
            }
            else
            {
                if (int.TryParse(timeprigot.Text, out int cookingTime))
                {
                    recipes.CookingTime = cookingTime;
                }
                else
                {
                    sb.Append("Время приготовления должно быть числом!\n");
                }
            }

            // Проверка изображения
            if (string.IsNullOrWhiteSpace(recipes.Image))
            {
                // Устанавливаем изображение по умолчанию
                recipes.Image = "Images/default_recipe.png";
            }

            // Если есть ошибки, показываем их
            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохранение в базу данных
            try
            {
                string message = "Изменения сохранены!";

                if (recipes.RecipeID == 0) // Новый рецепт
                {
                    AppConnect.model01.Recipes.Add(recipes);
                    message = "Рецепт успешно добавлен!";
                }
                else // Обновление существующего
                {
                    var existingRecipe = AppConnect.model01.Recipes.Find(recipes.RecipeID);
                    if (existingRecipe != null)
                    {
                        existingRecipe.RecipeName = recipes.RecipeName;
                        existingRecipe.Description = recipes.Description;
                        existingRecipe.CategoryID = recipes.CategoryID;
                        existingRecipe.AuthorID = recipes.AuthorID;
                        existingRecipe.CookingTime = recipes.CookingTime;
                        existingRecipe.Image = recipes.Image;
                        message = "Рецепт успешно обновлен!";
                    }
                    else
                    {
                        MessageBox.Show("Рецепт не найден в базе данных!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                AppConnect.model01.SaveChanges();
                MessageBox.Show(message, "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Возврат на страницу со списком рецептов
                AppFrame.Framemain.Navigate(new PageTasks());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении рецепта: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonback_Click(object sender, RoutedEventArgs e)
        {
            // Возврат на предыдущую страницу
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                AppFrame.Framemain.Navigate(new PageTasks());
            }
        }

        private void downloadimage_Click_1(object sender, RoutedEventArgs e)
        {
            AppFrame.Framemain.Navigate(new MagicHelper());

        }
    }
}
