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
        public virtual DbSet<MediaSource> MediaSources { get; set; }
        public virtual DbSet<TimeTable> TimeTables { get; set; }
        public virtual DbSet<User> Users { get; set; }
        // AnalyticLayer
        public virtual DbSet<AnalyticData> AnalyticDatas { get; set; }
        public virtual DbSet<AnalyticDefault> AnalyticDefaults { get; set; }
        public virtual DbSet<DataHasDefault> DataHasDefaults { get; set; }
        public virtual DbSet<DefaultDataInput> DefaultDataInputs { get; set; }
        public virtual DbSet<DefaultDataScore> DefaultDataScores { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        // ExamLayer
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamResult> ExamResults { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<UserEQLog> UserEQLogs { get; set; }
        // TimeLineLayer
        public virtual DbSet<TimeTableInfoHasMediaSrc> TimeTableInfoHasMediaSrcs { get; set; }
        public virtual DbSet<TimeTableInformation> TimeTableInformations { get; set; }
    }
}
