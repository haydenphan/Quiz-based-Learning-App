using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class QuizDbContextFactory : IDesignTimeDbContextFactory<QuizDbContext>
    {
        public QuizDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuizDbContext>();
            optionsBuilder.UseSqlServer("Server=LAPTOP-DVKPB8S9;Database=QuizzesDB;User Id=sa;Password=123;TrustServerCertificate=True");
            return new QuizDbContext(optionsBuilder.Options);
        }
    }
}
