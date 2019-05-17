using EduriaData.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eduria
{
    public class EduriaContext : IdentityDbContext
    {
        public EduriaContext(DbContextOptions options) : base(options) { }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
