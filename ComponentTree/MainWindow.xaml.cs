using System.Windows;

namespace ComponentTree
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }

        private void MenuItem_AddRoot(object sender, RoutedEventArgs e)
        {
            var form = new ComponentForm(null);
            form.ShowDialog();
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            // получить Id выьранного элемента, запустить форму с этим парамметром

        }
    }
}
