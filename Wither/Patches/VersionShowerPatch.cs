using System;
using HarmonyLib;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionShowerPatch
    {
        public static void Postfix()
        {
            string text = Reactor.Patches.ReactorVersionShower.Text.Text;
            int modsIndex = text.IndexOf("Mods", StringComparison.Ordinal);
            string substring = text.Substring(0, modsIndex);
            substring += Utils.StringNames.ModName + " v" + Utils.StringNames.ModVersion;
            substring += $"\n{text.Substring(modsIndex)}";
            Reactor.Patches.ReactorVersionShower.Text.Text = substring;
        }
    }
}