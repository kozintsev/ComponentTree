using System.Windows;
using ComponentTree.Model;

namespace ComponentTree
{
    /// <summary>
    /// Логика взаимодействия для ComponentForm.xaml
    /// </summary>
    public partial class ComponentForm
    {
        private readonly bool _isRename;
        private readonly ComponentViewModel _viewModel;

        /// <summary>
        /// Создаём корневой объект
        /// </summary>
        public ComponentForm()
        {
            InitializeComponent();
            _isRename = false;
            TbQ.Visibility = Visibility.Hidden;
            LabelQ.Visibility = Visibility.Hidden;
            _viewModel = (ComponentViewModel) DataContext;
        }

        /// <summary>
        /// Переименовать
        /// </summary>
        /// <param name="product"></param>
        /// <param name="isAdd"></param>
        /// <param name="isRename"></param>
        public ComponentForm(Product product, bool isAdd = false, bool isRename = false)
        {
            InitializeComponent();
            _viewModel = (ComponentViewModel)DataContext;
            _isRename = isRename;
            if (!isRename) return;
            TbDes.Text = product.Designation;
            TbName.Text = product.Name;
            TbQ.IsEnabled = false;
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Button_Ok(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbDes.Text))
            {
                LabelMessage.Content = "Введите обозначение";
                return;
            }

            if (!int.TryParse(TbQ.Text, out var result))
            {
                LabelMessage.Content = "Введите число в строку колличество";
                return;
            }

            var designation = TbDes.Text.Trim();
            var name = TbName.Text.Trim();

            _viewModel.Designation = designation;
            _viewModel.Name = name;

            if (_isRename)
            {
                await _viewModel.Rename(designation, name);
            }
            else
            {
                await _viewModel.Create(designation, name, result);
            }
            DialogResult = true;
            Close();
        }
    }
}
