using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ComponentTree.Model;

namespace ComponentTree
{
    /// <summary>
    /// Логика взаимодействия для ComponentForm.xaml
    /// </summary>
    public partial class ComponentForm
    {
        private readonly long? _parentId;
        private readonly bool _isRename;

        public Product Product { get; set; }

        /// <summary>
        /// Создаём корневой объект
        /// </summary>
        public ComponentForm()
        {
            InitializeComponent();
            _parentId = null;
            Product = new Product();
            _isRename = false;
            TbQ.Visibility = Visibility.Hidden;
            LabelQ.Visibility = Visibility.Hidden;
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
            Product = product;
            if (isAdd) _parentId = product.Id;
                else
                _parentId = null;
            _isRename = isRename;
            if (!isRename) return;
            tbDes.Text = product.Designation;
            tbName.Text = product.Name;
            TbQ.IsEnabled = false;
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async Task Create(string designation, string name, int q)
        {
            var context = new Components();

            var find = context.Component.FirstOrDefault(x => x.Designation == designation);

            Component com;
            // Ищем компоненте если не находим создаём новый (ищем по обозначению)
            if (find == null)
            {
                com = new Component()
                {
                    Name = name,
                    Designation = designation,
                };

                context.Component.Add(com);
                await context.SaveChangesAsync();
            }
            else
            {
                com = find;
            }

            if (_parentId == com.Id)
            {
                LabelMessage.Content = "Элемент не может ссылаться на себя";
                return;
            }

            var link = new Link
            {
                IdParent = _parentId,
                IdChild = com.Id,
                Quantity = 1
            };

            if (q > 1)
            {
                link.Quantity = q;
            }

            context.Link.Add(link);

            await context.SaveChangesAsync();

            var product = new Product
            {
                Designation = designation,
                Name = name,
                Quantity = q
            };

            if (Product.ProductCollection != null)
            {
                Product.ProductCollection.Add(product);
            }
            else
            {
                Product.ProductCollection = new ObservableCollection<Product> {product};
            }

        }

        private async Task Rename(string designation, string name)
        {
            using (var context = new Components())
            {
                var find = context.Component.FirstOrDefault(x => x.Designation == designation);
                if (find != null)
                {
                    find.Designation = designation;
                    find.Name = name;
                    await context.SaveChangesAsync();
                    Product.Designation = designation;
                    Product.Name = name;
                }
            }
        }

        private async void Button_Ok(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbDes.Text))
            {
                LabelMessage.Content = "Введите обозначение";
                return;
            }

            if (!int.TryParse(TbQ.Text, out var result))
            {
                LabelMessage.Content = "Введите число в строку колличество";
                return;
            }

            var designation = tbDes.Text.Trim();
            var name = tbName.Text.Trim();

            if (_isRename)
            {
                await Rename(designation, name);
            }
            else
            {
                await Create(designation, name, result);
            }
            DialogResult = true;
            Close();
        }
    }
}
