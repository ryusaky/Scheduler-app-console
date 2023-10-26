using Microsoft.EntityFrameworkCore;

namespace ConsoleAppScheduler.DataBase
{
    public class PIDbContext : DbContext
    {
        public PIDbContext(DbContextOptions<PIDbContext> options) : base(options)
        {
        }
    }
}
