using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EntityLinqBeginProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using(AppContext appContext = new AppContext())
            {
                appContext.Database.EnsureDeleted();
                appContext.Database.EnsureCreated();

                List<Company> companies = new();
                companies.Add(new Company() { Name = "Yandex" });
                companies.Add(new Company() { Name = "Mail Group" });
                appContext.Companies.AddRange(companies);

                List<User> users = new();
                users.Add(new User() { Name = "Bob", Company = companies[0], Age = 35 });
                users.Add(new User() { Name = "Joe", Company = companies[0], Age = 21 });
                users.Add(new User() { Name = "Tim", Company = companies[1], Age = 40 });
                users.Add(new User() { Name = "Sam", Company = companies[1], Age = 32 });
                users.Add(new User() { Name = "Joe", Company = companies[1], Age = 44 });
                appContext.Users.AddRange(users);

                appContext.SaveChanges();
            }

            using (AppContext appContext = new AppContext())
            {
                var company = appContext.Companies.FirstOrDefault();
                //var users = (from user in appContext.Users.Include(u => u.Company)
                //             where user.CompanyId == company.Id
                //             select user).ToList();

                var users1 = appContext.Users
                                        .Where(u => u.Company!.Name == company!.Name)
                                        .ToList();

                Console.WriteLine(company!.Name);
                foreach(var user in users1)
                    Console.WriteLine($"\t{user.Name} {user.Age}");
                Console.WriteLine("---------------------");

                var users2 = (from user in appContext.Users
                              where user.Company!.Name == company.Name
                              select user).ToList();

                Console.WriteLine(company.Name);
                foreach (var user in users2)
                    Console.WriteLine($"\t{user.Name} {user.Age}");
                Console.WriteLine("---------------------");

                var users3 = appContext.Users.Where(u => EF.Functions.Like(u.Name!, "[^J]o%")).ToList();
                foreach (var user in users3)
                    Console.WriteLine($"\t{user.Name} {user.Age}");
                Console.WriteLine("---------------------");

                User? user1 = appContext.Users.Find(5);
                if(user1 != null)
                    Console.WriteLine($"\t{user1!.Name} {user1!.Age}");

                User? user2 = appContext.Users.FirstOrDefault(u => u.Name == "Joe");
                if (user2 != null)
                    Console.WriteLine($"\t{user2!.Name} {user2!.Age}");

                User? user3 = appContext.Users.SingleOrDefault(u => u.Name == "Tim");
                if (user3 != null)
                    Console.WriteLine($"\t{user3!.Name} {user3!.Age}");

                User? user4 = appContext.Users.OrderBy(u => u.Age).LastOrDefault(u => u.Name == "Joe");
                if (user4 != null)
                    Console.WriteLine($"\t{user4!.Name} {user4!.Age}");

                var users4 = appContext.Users
                              .Join(appContext.Companies,
                              u => u.CompanyId,
                              c => c.Id,
                              (u, c) => new
                              {
                                  Name = u.Name,
                                  Age = u.Age,
                                  Company = c.Name
                              })
                              .OrderBy(a => a.Company)
                              .ThenBy(a => a.Age);


                var users5 = from user in appContext.Users
                             join comp in appContext.Companies on user.CompanyId equals comp.Id
                             orderby comp.Name
                             orderby user.Age
                             select new
                             {
                                 Name = user.Name,
                                 Age = user.Age,
                                 Company = comp.Name
                             };
                             
                              
                foreach (var user in users4)
                    Console.WriteLine($"{user.Name} {user.Age} {user.Company}");
                Console.WriteLine("---------------------");

                foreach (var user in users5)
                    Console.WriteLine($"{user.Name} {user.Age} {user.Company}");
                Console.WriteLine("---------------------");

            }
        }
    }
}