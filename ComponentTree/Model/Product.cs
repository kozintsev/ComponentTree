using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ComponentTree.Model
{
    public class Product : INotifyPropertyChanged
    {
        public Product()
        {
            ProductCollection = new ObservableCollection<Product>();
        }

        public ObservableCollection<Product> ProductCollection { get; set; }

        public long Id { get; set; }

        private string _designation;
        public string Designation
        {
            get => _designation;
            set
            {
                _designation = value;
                OnPropertyChanged("Designation");

            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _hyphen;
        public string Hyphen
        {
            get
            {
                _hyphen = string.IsNullOrEmpty(Name) ? "" : " - ";
                return _hyphen;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
