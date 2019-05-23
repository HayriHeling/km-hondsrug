using EduriaData.Models;
using Microsoft.EntityFrameworkCore;

namespace Eduria
{
    public class EduriaContext : DbContext
    {
        public EduriaContext(DbContextOptions options) : base(options) { }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserExam> UserExams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<UserEQLog> UserEQLogs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
