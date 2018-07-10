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
        private Product _selectProduct;
        private readonly ApplicationViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ApplicationViewModel();
            DataContext = _viewModel;
            ProductsTreeView.ItemsSource = _viewModel.Products;
            CollectionViewSource.GetDefaultView(ProductsTreeView.ItemsSource).Refresh();
        }

        private void MenuItem_AddRoot(object sender, RoutedEventArgs e)
        {
            var cvm = new ComponentViewModel();
            var form = new ComponentForm
            {
                DataContext = cvm,
                Owner = this
            };
            var result = form.ShowDialog();
            if(!result.HasValue) return;
            if(!result.Value) return;

            if (cvm.Product == null) return;
            _viewModel.Products.Add(cvm.Product);
            CollectionViewSource.GetDefaultView(ProductsTreeView.ItemsSource).Refresh();
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            // получить Id выьранного элемента, запустить форму с этим парамметром
            if (_selectProduct == null) return;
            var form = new ComponentForm(_selectProduct, true)
            {
                DataContext = new ComponentViewModel(_selectProduct, true),
                Owner = this
            };
            var result = form.ShowDialog();

            if (!result.HasValue) return;
            if (!result.Value) return;

            CollectionViewSource.GetDefaultView(ProductsTreeView.ItemsSource).Refresh();
        }

        private void MenuItem_CreateReport(object sender, RoutedEventArgs e)
        {
            if (_selectProduct == null) return;
            // todo: получаем список для вывода в MS Word / LibreOffice или куда-то ещё
            var list = _viewModel.SearchSpecification(_selectProduct.Id);
        }

        private void MenuItem_Rename(object sender, RoutedEventArgs e)
        {
            if (_selectProduct == null) return;
            var form = new ComponentForm(_selectProduct, false, true)
            {
                DataContext = new ComponentViewModel(_selectProduct, false, true),
                Owner = this
            };
            var result = form.ShowDialog();

            if (!result.HasValue) return;
            if (!result.Value) return;

            CollectionViewSource.GetDefaultView(ProductsTreeView.ItemsSource).Refresh();
        }

        private void MenuItem_Delete(object sender, RoutedEventArgs e)
        {
            if (_selectProduct == null) return;
            _viewModel.Delete(_selectProduct.Id);
        }

        private void ProductsTreeView_OnSelected(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is TreeViewItem item)) return;
            if (item.DataContext is Product selectedClass)
            {
                _selectProduct = selectedClass;
                var products = selectedClass.ProductCollection;
                
            }
        }
    }
}
