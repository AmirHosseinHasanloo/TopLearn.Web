using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<TopLearnContext>
    {
        public TopLearnContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TopLearnContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=TopLearn_DB;User Id=sa;Password=asad@1384;TrustServerCertificate=True");

            return new TopLearnContext(optionsBuilder.Options);
        }
    }
}
