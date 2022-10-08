using System.Data.Entity;
using System.Diagnostics;

namespace DAL
{
    public class DataContext : DbContext
    {

        public DataContext()
            : base("Home_Media_Player")
        {
            Database.Log = s => Debug.WriteLine(s);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, DAL.Migrations.Configuration>());
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public DbSet<Albums> Albums { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Files> Files { get; set; }
    }
}
