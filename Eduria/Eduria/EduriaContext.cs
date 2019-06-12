using EduriaData.Models;
using EduriaData.Models.AnalyticLayer;
using EduriaData.Models.ExamLayer;
using EduriaData.Models.TimeLineLayer;
using Microsoft.EntityFrameworkCore;

namespace Eduria
{
    public class EduriaContext : DbContext
    {
        public EduriaContext(DbContextOptions options) : base(options) { }
        // Models
        public DbSet<MediaSource> MediaSources { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<User> Users { get; set; }
        // AnalyticLayer
        public virtual DbSet<AnalyticData> AnalyticDatas { get; set; }
        public virtual DbSet<AnalyticDefault> AnalyticDefaults { get; set; }
        public virtual DbSet<DataHasDefault> DataHasDefaults { get; set; }
        public DbSet<DefaultDataInput> DefaultDataInputs { get; set; }
        public DbSet<DefaultDataScore> DefaultDataScores { get; set; }
        public DbSet<Period> Periods { get; set; }
        // ExamLayer
        public DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<UserEQLog> UserEQLogs { get; set; }
        // TimeLineLayer
        public DbSet<TimeLine> TimeLines { get; set; }
        public DbSet<TimeLineHasTimeTable> TimeLineHasTimeTables { get; set; }
        public DbSet<TimeLineHasUser> TimeLineHasUsers { get; set; }
        public DbSet<TimeTableInfoHasMediaSrc> TimeTableInfoHasMediaSrcs { get; set; }
        public DbSet<TimeTableInformation> TimeTableInformations { get; set; }
    }
}
