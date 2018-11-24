using System.Collections.Generic;
using dynperf_server.Models;

namespace dynperf_server.Domain
{
    public static class DefaultTargetDefinition
    {
        public static readonly List<TargetProcessEntry> DefaultsList = new List<TargetProcessEntry>()
        {
            new TargetProcessEntry("League of Legen") { Description = "League of Legends" },
        };
    }
}