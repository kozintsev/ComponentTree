using System.Data.Entity;

namespace ComponentTree.Model
{
    public class Components : DbContext
    {
        public Components() : base("components")
        {
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<Components>());
        }

        public DbSet<Component> Component { get; set; }
        public DbSet<Link> Link { get; set; }
    }
}
