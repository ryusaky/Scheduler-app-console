using Microsoft.EntityFrameworkCore;

namespace SICOVIN_CODE_SCHEDULER.Database
{
    public class SICOVINDbContext : DbContext
    {
        public DbSet<JobDescription> JobDescriptions { get; set; }
        public SICOVINDbContext(DbContextOptions<SICOVINDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobDescription>()
                .Property(e => e.JobId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<JobDescription>()
                .HasKey(e => e.JobId);
        }
    }
}
