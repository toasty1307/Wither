using System;
using System.Reflection;
using HarmonyLib;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(VersionShower))]
    public static class VersionShowerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(VersionShower.Start))]
        public static void Start(VersionShower __instance)
        {
            string text = Reactor.Patches.ReactorVersionShower.Text.text;
            int modsIndex = text.IndexOf("Mods", StringComparison.Ordinal);
            string substring = text.Substring(0, modsIndex);
            substring += "Wither" + " v" + typeof(WitherPlugin).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            substring += $"\n{text.Substring(modsIndex)}";
            Reactor.Patches.ReactorVersionShower.Text.text = substring;
        }
    }
}