using HarmonyLib;
using TMPro;
using UnityEngine;
using Wither.Utils;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(PingTracker))]
    public static class PingTrackerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        public static void Update(PingTracker __instance)
        {
            __instance.text.alignment = TextAlignmentOptions.Baseline;
            __instance.text.text += $"\n\nWither Mod by-\ntoasty marshmallow\n({CoolMethods.GetLinkText("https://github.com/ToastyMarshmallow", "Github", Color.blue)})";
        }
    }
}