using System.Data;

namespace ConsoleAppScheduler.Base.TemplateMapper
{
    public class DataParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public object Type { get; set; }
        public ParameterDirection Direction { get; set; }
        public bool? IsNullable { get; set; }
        public int? Size { get; set; }
        public object DefaultValue { get; set; }
    }
}
