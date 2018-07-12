using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ComponentTree.Model;
using Component = ComponentTree.Model.Component;

namespace ComponentTree
{
    public class ComponentViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _designation;
        private string _message;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Designation
        {
            get => _designation;
            set
            {
                _designation = value;
                OnPropertyChanged("Designation");
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public bool IsRename { get; }

        private readonly long? _parentId;

        public Product Product { get; set; }

        public ComponentViewModel()
        {
            _parentId = null;
            Product = new Product();
            IsRename = false;
        }

        public ComponentViewModel(Product product, bool isAdd = false, bool isRename = false)
        {
            Product = product;
            if (isAdd) _parentId = product.Id;
            else
                _parentId = null;
            IsRename = isRename;
            if (!isRename) return;
            Designation = product.Designation;
            Name = product.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task<string> Create(string designation, string name, int q)
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
                Message = "Элемент не может ссылаться на себя";
                return Message;
            }

            //todo: нет проверки, когда есть связь элемента А с Б, но мы создаём точно такую же связь,
            //должно возникать сообщение или увеличение колличества

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

            Product = product;

            return string.Empty;
        }

        public async Task Rename(string designation, string name)
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
    }
}
