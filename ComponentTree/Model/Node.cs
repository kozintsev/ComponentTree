using System.Collections.ObjectModel;

namespace ComponentTree.Model
{
    public class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }
    }

}
