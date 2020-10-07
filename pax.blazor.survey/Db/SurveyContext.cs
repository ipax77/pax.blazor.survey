using Microsoft.EntityFrameworkCore;
using pax.blazor.survey.Models;

namespace pax.blazor.survey.Db
{
    public class SurveyContext : DbContext
    {
        public SurveyContext(DbContextOptions<SurveyContext> options) : base(options)
        {

        }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Response>(entity =>
            {
                entity.HasKey(k => k.ID);
                entity.HasIndex(i => i.Pos);
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(k => k.ID);
                entity.HasIndex(e => new { e.Pos });
                entity.HasIndex(e => new { e.Selected });
            });
        }
    }

}
