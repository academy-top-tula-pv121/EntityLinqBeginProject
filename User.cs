using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLinqBeginProject
{
    public class User
    {
        public int Id { set; get; }
        public string? Name { set; get; }
        public int? Age { set; get; }

        public int CompanyId { set; get; }
        public Company? Company { set; get; }
    }

    public class Company
    {
        public int Id { set; get; }
        public string? Name { set; get; }
        public List<User> Users { set; get; } = new();
    }

    public class AppContext : DbContext
    {
        public DbSet<User> Users { set; get; } = null!;
        public DbSet<Company> Companies { set; get; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Initial Catalog=UsersDb;Integrated Security=True");
        }
    }

}
