using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SICOVIN_CODE_SCHEDULER.Database
{
    [Table(name: "JobDescription")]
    public class JobDescription
    {
        [Key]
        public int JobId { get; set; }

        public string JobName { get; set; }
        public string CronExpression { get; set; }

    }
}
