namespace dynperf_server.Models
{
    public class TargetProcessEntry
    {
        public TargetProcessEntry() { }

        public TargetProcessEntry(string processName)
        {
            ProcessName = processName;
        }

        public string ProcessName { get; set; }
        public string Description { get; set; }

    }
}