using Microsoft.EntityFrameworkCore;

namespace LoginLogout.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<PasswordCode> PasswordCode { get; set; }
    }
}
