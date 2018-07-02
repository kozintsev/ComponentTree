using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ComponentTree.Model;

namespace ComponentTree
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private Product _selectedProduct;

        public ObservableCollection<Product> Products { get; set; }
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(@"SelectedProduct");
            }
        }

        public ApplicationViewModel()
        {
            Products = new ObservableCollection<Product>
            {
                new Product { Name= "Вал", Designation= "ААА.001.001.001", },
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
