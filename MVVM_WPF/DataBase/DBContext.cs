using System.Data.Entity;

namespace MVVMSQLiteApp
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DefaultConnection") {}
        public DbSet<UserData> UDataCollection { get; set; }
    }
}