using System.Collections.Generic;
using dynperf.Models;

namespace dynperf.Builders
{
    public static class DefaultTargets
    {
        private static readonly List<TargetProcessEntry> defaults = new List<TargetProcessEntry>()
        {
            new TargetProcessEntry("gameoverlayui", "Steam (Game Overlay)"),
            new TargetProcessEntry("vkcube", "vulkan test application"),

            new TargetProcessEntry("retroarch", "Retroarch Emulator Frontend"),
            new TargetProcessEntry("PCSX2", "PlayStation 2 Emualtor"),
            new TargetProcessEntry("rpcs3", "PlayStation 3 Emulator"),
            new TargetProcessEntry("cemu", "Wii U mulator"),
            new TargetProcessEntry("yuzu", "Switch Emulator"),

            new TargetProcessEntry("League of Legends"),
            new TargetProcessEntry("Civ6", "Civilization VI"),
            new TargetProcessEntry("ShadowOfTheTombRaider"),

            new TargetProcessEntry("Titanfall2.exe"),
            new TargetProcessEntry("bfv.exe", "Battlefield V"),
            new TargetProcessEntry("bfvTrial.exe", "Battlefield V (Trial)"),
            new TargetProcessEntry("sekiro.exe", "Sekiro - Shadow Die Twice"),
            new TargetProcessEntry("Vainglory.exe"),
        };

        public static IReadOnlyCollection<TargetProcessEntry> GetDefaults()
        {
            return defaults.AsReadOnly();
        }
    }
}