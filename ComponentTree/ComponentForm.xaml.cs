using System.Linq;
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

        public Product Product { get; set; }

        /// <summary>
        /// Создаём корневой объект
        /// </summary>
        public ComponentForm()
        {
            InitializeComponent();
            _parentId = null;
            Product = null;
        }

        /// <summary>
        /// Создать объект
        /// </summary>
        /// <param name="parentId"></param>
        public ComponentForm(long parentId)
        {
            InitializeComponent();
            _parentId = parentId;
            Product = null;
        }
        
        /// <summary>
        /// Переименовать
        /// </summary>
        /// <param name="product"></param>
        public ComponentForm(Product product)
        {
            InitializeComponent();
            Product = product;
            _parentId = null;
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Create()
        {

        }

        private void Rename()
        {

        }

        private void Button_Ok(object sender, RoutedEventArgs e)
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
                context.SaveChanges();
            }
            else
            {
                com = find;
            }

            Product = new Product {Id = com.Id, Designation = com.Designation, Name = com.Name};

            var link = new Link
            {
                IdParent = _parentId,
                IdChild = com.Id,
                Quantity = 1
            };

            if (result > 1)
            {
                link.Quantity = result;
            }

            context.Link.Add(link);

            context.SaveChanges();

            DialogResult = true;

            Close();
        }
    }
}
