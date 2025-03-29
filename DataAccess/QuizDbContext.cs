using DataAccess.Models;
using DataAccess.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class QuizDbContext : IdentityDbContext<IdentityUser>
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options)
            : base(options)
        {
        }

        public DbSet<Option> Options { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<TimeLimit> TimeLimits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Option>()
                .HasKey(o => o.Id);
            modelBuilder.Entity<Option>()
                .HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuestionID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasKey(q => q.Id);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(q => q.QuizID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.TimeLimit)
                .WithMany(t => t.Questions)
                .HasForeignKey(q => q.TimeLimitID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quiz>()
                .HasKey(q => q.Id);
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.User)
                .WithMany()
                .HasForeignKey(q => q.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TimeLimit>()
                .HasKey(t => t.Id);
        }

        #region Repositories registering

        public Repository<Option> OptionRepository => new Repository<Option>(this);
        public Repository<Question> QuestionRepository => new Repository<Question>(this);
        public Repository<Quiz> QuizRepository => new Repository<Quiz>(this);
        private Repository<TimeLimit> _timeLimitRepository;
        public Repository<TimeLimit> TimeLimitRepository
        {
            get
            {
                if (_timeLimitRepository == null)
                {
                    _timeLimitRepository = new Repository<TimeLimit>(this);
                }
                return _timeLimitRepository;
            }
        }

        #endregion
    }
}