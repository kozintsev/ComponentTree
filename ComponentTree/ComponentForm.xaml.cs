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

        public ComponentForm(long? parentId)
        {
            InitializeComponent();
            _parentId = parentId;
            Product = null;
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
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

            var context = new Components();

            var com = new Component()
            {
                Name = tbName.Text,
                Designation = tbDes.Text,
            };

            context.Component.Add(com);
            context.SaveChanges();

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
