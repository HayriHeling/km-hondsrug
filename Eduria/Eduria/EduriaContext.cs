using EduriaData.Models;
using EduriaData.Models.ExamLayer;
using EduriaData.Models.AnalyticLayer;
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
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<UserEQLog> UserEQLogs { get; set; }
        public DbSet<AnswerT> AnswerTs { get; set; }
        public DbSet<QuestionHasAnswerT> QuestionHasAnswerTs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AnalyticData> AnalyticDatas { get; set; }
        public DbSet<AnalyticGoal> AnalyticGoals { get; set; }
        public DbSet<AnalyticMethod> AnalyticMethods { get; set; }
    }
}
