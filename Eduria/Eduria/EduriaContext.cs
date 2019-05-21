using EduriaData.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eduria
{
    public class EduriaContext : DbContext
    {
        public EduriaContext(DbContextOptions options) : base(options) { }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserTest> UserTests { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<UserTQLog> UserTQLogs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
