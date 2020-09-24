namespace dynperf.Models
{
    public class TargetProcessEntry
    {
        public TargetProcessEntry() { }

        public TargetProcessEntry(string processName)
        {
            ProcessName = processName;
        }

        public TargetProcessEntry(string processName, string description)
        {
            ProcessName = processName;
            Description = description;
        }

        public string ProcessName { get; set; }
        public string Description { get; set; }
    }
}