using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ComponentTree.Model;

namespace ComponentTree
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private TreeViewItem _selectedItem;
        private Product _selectProduct;

        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new ApplicationViewModel();
            DataContext = viewModel;
            ProductsTreeView.ItemsSource = viewModel.Products;
            CollectionViewSource.GetDefaultView(ProductsTreeView.ItemsSource).Refresh();
        }

        private void MenuItem_AddRoot(object sender, RoutedEventArgs e)
        {
            var form = new ComponentForm(null);
            form.ShowDialog();
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            // получить Id выьранного элемента, запустить форму с этим парамметром
            if (_selectProduct == null) return;
            var form = new ComponentForm(_selectProduct.Id);
            form.ShowDialog();
        }

        private void ProductsTreeView_OnSelected(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is TreeViewItem item)) return;
            if (item.DataContext is Product selectedClass)
            {
                _selectProduct = selectedClass;
                var products = selectedClass.ProductCollection;
                
            }
            _selectedItem = item;
        }
    }
}
