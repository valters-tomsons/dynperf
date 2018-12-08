using System.Collections.Generic;
using dynperf_server.Models;

namespace dynperf_server.Domain
{
    public static class DefaultTargetDefinition
    {
        public static readonly List<TargetProcessEntry> DefaultsList = new List<TargetProcessEntry>()
        {
            new TargetProcessEntry("League of Legen") { Description = "League of Legends" },
            new TargetProcessEntry("Cemu") { Description = "Cemu (Wii U Emulator)"},
            new TargetProcessEntry("rpcs3") { Description = "RPCS3 (Playstation 3 Emulator)"},
            new TargetProcessEntry("retroarch") { Description = "Retroarch Emulator Frontend"},
            new TargetProcessEntry("Vainglory.exe") { Description = "Vainglory"},
            new TargetProcessEntry("gameoverlayui") { Description = "Steam In-Game Overlay"},
        };
    }
}