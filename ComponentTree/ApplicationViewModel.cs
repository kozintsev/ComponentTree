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
        public ObservableCollection<Product> Products { get; set; }

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
        /// <param name="context"></param>
        /// <param name="component"></param>
        /// <param name="product"></param>
        public void LazyTreeLoader(Components context, Model.Component component , Product product)
        {
            var linkChildren = context.Link.Where(x => x.IdParent == component.Id).ToList();
            product.ProductCollection = new ObservableCollection<Product>();

            foreach (var link in linkChildren)
            {
                // одна связь связывает 1 компонент
                var childComponent = context.Component.FirstOrDefault(x => x.Id == link.IdChild);
                if (childComponent == null) continue;
                var p = new Product
                {
                    Id = childComponent.Id,
                    Designation = childComponent.Designation,
                    Name = childComponent.Name,
                    Quantity = link.Quantity
                };
                product.ProductCollection.Add(p);
                LazyTreeLoader(context, childComponent, p);
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
                        if (childComponent == null) continue;
                        var p = new Product
                        {
                            Id = childComponent.Id,
                            Designation = childComponent.Designation,
                            Name = childComponent.Name,
                            Quantity = link.Quantity
                        };
                        prodColl.Add(p);
                        LazyTreeLoader(context, childComponent, p);
                    }

                    Products.Add(new Product
                    {
                        Id = component.Id,
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
