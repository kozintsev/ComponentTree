using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public void LazyTreeLoader(List<Model.Component> components)
        {
            foreach (var item in components)
            {
                
            }
        }

        public ApplicationViewModel()
        {
            var context = new Components();

            var rootLinks = context.Link.Where(x => x.IdParent == null);

            Products = new ObservableCollection<Product>();

            foreach (var rootLink in rootLinks)
            {
                var components = context.Component.Where(x => x.Id == rootLink.IdChild).ToList();
                foreach (var component in components)
                {
                    // получаем детей объекта
                    var linkChildren = context.Link.Where(x => x.IdParent == component.Id).ToList();

                    var prodColl = new ObservableCollection<Product>();

                    foreach (var link in linkChildren)
                    {
                        var childComponent = context.Component.FirstOrDefault(x => x.Id == link.IdChild);
                        if (childComponent != null)
                            prodColl.Add(new Product
                            {
                                Id = childComponent.Id,
                                Designation = childComponent.Designation,
                                Name = childComponent.Name
                            });
                    }

                    Products.Add(new Product
                    {
                        Designation = component.Designation,
                        Name = component.Name,
                        ProductCollection = prodColl
                    });
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
