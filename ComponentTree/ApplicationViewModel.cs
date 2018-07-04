﻿using System.Collections.Generic;
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

        private static ObservableCollection<Product> Find(long id, ObservableCollection<Product> listProducts)
        {
            foreach (var product in listProducts)
            {
                if (product.Id == id)
                    return product.ProductCollection;
                var find = Find(id, product.ProductCollection);
                if (find != null)
                    return find;
            }
            return null;
        }

        /// <summary>
        /// Формируем список для вывода в отчёт. Ищем вниз по дереву рекурсивно
        /// </summary>
        /// <param name="id"></param>
        public ObservableCollection<Product> SearchSpecification(long id)
        {
            ObservableCollection<Product> pr = null;
            foreach (var product in Products)
            {
                if (product.Id == id)
                {
                    return product.ProductCollection;
                }
                pr = Find(id, product.ProductCollection);
            }
            return pr;
        }

        /// <summary>
        /// Метод для загрузки дерева в память. Необходимо оптимизация! Оптимально подгружать дерево по мере раскрытия дерева
        /// с автоподгрузкой 1-2 уровней
        /// </summary>
        /// <param name="components"></param>
        public void LazyTreeLoader(List<Model.Component> components)
        {
            foreach (var item in components)
            {
                
            }
        }

        /// <summary>
        /// Рекурсивное удаление объектов из дерева, которое строится в памяти для отображении на TreeView
        /// </summary>
        /// <param name="products"></param>
        /// <param name="id"></param>
        private static void DeleteInTree(ICollection<Product> products, long id)
        {
            foreach (var item in products)
            {
                if (item.Id == id)
                {
                    products.Remove(item);
                    break;
                }
                DeleteInTree(item.ProductCollection, id);
            }
        }

        public void Delete(long id)
        {
            using (var context = new Components())
            {
                var obj = context.Component.FirstOrDefault(x => x.Id == id);
                if (obj != null)
                    context.Component.Remove(obj);
                // todo: здесь также нужно удалить все связи и выполнить проверку, вообще можем ли мы удалить этот объект
                // или необходимо удалить только связи
            }

            foreach (var product in Products)
            {
                if (product.Id == id)
                {
                    Products.Remove(product);
                    break;
                }
                DeleteInTree(product.ProductCollection, id);
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
                        // одна связь связывает 1 компонент
                        var childComponent = context.Component.FirstOrDefault(x => x.Id == link.IdChild);
                        if (childComponent != null)
                            prodColl.Add(new Product
                            {
                                Id = childComponent.Id,
                                Designation = childComponent.Designation,
                                Name = childComponent.Name,
                                Quantity = link.Quantity
                            });
                    }

                    Products.Add(new Product
                    {
                        Designation = component.Designation,
                        Name = component.Name,
                        ProductCollection = prodColl,
                        Quantity = rootLink.Quantity
                    });

                    // todo: тут мы должны получать детей-детей рекурсивно и добавлять их в
                    // Products для этого можно использовать метод LazyTreeLoader
                }
            }
            context.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
