using System;
using HarmonyLib;
using Wither.Utils;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionShowerPatch
    {
        public static void Postfix()
        {
            AddVersionToMainMenu();
            AssetBundleLoader.LoadBundles();
        }
        
        public static void AddVersionToMainMenu()
        {
            string text = Reactor.Patches.ReactorVersionShower.Text.Text;
            int modsIndex = text.IndexOf("Mods", StringComparison.Ordinal);
            string substring = text.Substring(0, modsIndex);
            substring += "Wither v1.0.0";
            substring += $"\n{text.Substring(modsIndex)}";
            Reactor.Patches.ReactorVersionShower.Text.Text = substring;
        }
    }
}