using HarmonyLib;
using UnityEngine;
using Wither.Utils;

namespace Wither.Patches
{
    [HarmonyPatch]
    public static class AnnouncementPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(AnnouncementPopUp._Show_d__16), nameof(AnnouncementPopUp._Show_d__16.MoveNext))]
        public static void OnEnable(AnnouncementPopUp._Show_d__16 __instance)
        {
            __instance.__this.AnnounceText.text = $"Wither mod?\nMade by toasty marshmallow#8535-{CoolMethods.GetLinkText("https://github.com/ToastyMarshmallow", "Github", Color.blue)}";
        }
    }
}