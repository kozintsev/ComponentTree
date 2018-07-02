using System.Collections.ObjectModel;
using System.Windows;
using ComponentTree.Model;

namespace ComponentTree
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ApplicationViewModel();

            //var context = new Components();

            //var com = new Component()
            //{
            //    Name = "Вал"
            //};

            //context.Component.Add(com);
            //context.SaveChanges();

            //var lin = new Link
            //{
            //    IdParent = null,
            //    IdChild = 3
            //};

            //context.Link.Add(lin);

            //context.SaveChanges();
        }

        private void MenuItem_AddRoot(object sender, RoutedEventArgs e)
        {
            //throw new System.NotImplementedException();
            var form = new ComponentForm(null);
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            //throw new System.NotImplementedException();
        }
    }
}
