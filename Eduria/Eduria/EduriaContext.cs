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
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<UserEQLog> UserEQLogs { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AnalyticData> AnalyticDatas { get; set; }
        public DbSet<AnalyticDefault> AnalyticDefaults { get; set; }
        public DbSet<DataHasDefault> DataHasDefaults { get; set; }
        public DbSet<DefaultDataInput> DefaultDataInputs { get; set; }
        public DbSet<DefaultDataScore> DefaultDataScores { get; set; }
    }
}
