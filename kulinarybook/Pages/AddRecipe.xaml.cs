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
    /// Логика взаимодействия для AddRecipe.xaml
    /// </summary>
    public partial class AddRecipe : Page
    {
        public Recipes recipes = new Recipes();
        public AddRecipe(Recipes recipe)
        {
            InitializeComponent();
            Fill();
            if (recipe != null)
            {
                recipes = recipe;
            }

            DataContext = recipes;
        }
        public void Fill()
        {
            var category = AppConnect.model01.Categories;
            categorycc.Items.Add("Категория");
            foreach (var item in category)
            { categorycc.Items.Add(item.CategoryName); }
            categorycc.SelectedIndex = 0;
            var authors = AppConnect.model01.Authors;
            authorcc.Items.Add("Авторы");
            foreach (var item in authors)
            { authorcc.Items.Add(item.AuthorName); }
            authorcc.SelectedIndex = 0;
        }

        private void namereciep_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void savereciep_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(recipes.RecipeName))
            {
                sb.Append("Добавьте название!\n");
            }
            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString());
            }
            else
            {
                string messag = "Изменения сохранены!";
                if (recipes.RecipeID == 0)
                {
                    recipes.CategoryID = AppData.AppConnect.model01.Categories.FirstOrDefault(x => x.CategoryName == categorycc.Text).CategoryID;
                    recipes.AuthorID = AppData.AppConnect.model01.Authors.FirstOrDefault(x => x.AuthorName == authorcc.Text).AuthorID;

                    AppConnect.model01.Recipes.Add(recipes);
                    messag = "Запись добавлена!";
                }
                try
                {
                    AppConnect.model01.SaveChanges();
                    MessageBox.Show(messag);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            AppFrame.Framemain.Navigate(new PageTasks());
        }
    }
}
