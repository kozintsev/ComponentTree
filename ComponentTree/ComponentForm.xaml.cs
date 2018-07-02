using System.Windows;
using ComponentTree.Model;

namespace ComponentTree
{
    /// <summary>
    /// Логика взаимодействия для ComponentForm.xaml
    /// </summary>
    public partial class ComponentForm : Window
    {
        private readonly long? _parentId;

        public ComponentForm(long? parentId)
        {
            InitializeComponent();
            _parentId = parentId;
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

            var text = $"{tbDes.Text} - {tbName.Text}"; 

            var context = new Components();

            var com = new Component()
            {
                Name = text
            };

            context.Component.Add(com);
            context.SaveChanges();

            var link = new Link
            {
                IdParent = _parentId,
                IdChild = com.Id
            };

            context.Link.Add(link);

            context.SaveChanges();
            Close();
        }
    }
}
