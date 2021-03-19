using System;
using HarmonyLib;
using Reactor.Extensions;
using UnityEngine;
using Wither.MonoBehaviour;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionShowerPatch
    {
        public static void Postfix()
        {
            AddVersionToMainMenu();
            MakeNewAlwaysActiveGameObject();
        }

        private static void MakeNewAlwaysActiveGameObject()
        {
            if (AlwaysActive.Instance != null) return;
            GameObject gameObject = new GameObject("AlwaysActive");
            gameObject.AddComponent<AlwaysActive>();
            gameObject.DontDestroyOnLoad();
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