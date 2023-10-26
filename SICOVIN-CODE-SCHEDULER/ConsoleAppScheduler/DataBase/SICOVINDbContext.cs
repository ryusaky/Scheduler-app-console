using Microsoft.EntityFrameworkCore;

namespace ConsoleAppScheduler.DataBase
{
    public class SICOVINDbContext : DbContext
    {
        public SICOVINDbContext(DbContextOptions<SICOVINDbContext> options) : base(options)
        {

        }
    }
}
