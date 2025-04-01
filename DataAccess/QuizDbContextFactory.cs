using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess
{
    public class QuizDbContextFactory : IDesignTimeDbContextFactory<QuizDbContext>
    {
        public QuizDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuizDbContext>();
            optionsBuilder.UseSqlServer("Server=(local);Database=QuizDb;Uid=sa; Pwd=12345; TrustServerCertificate=True");
            return new QuizDbContext(optionsBuilder.Options);
        }
    }
}
