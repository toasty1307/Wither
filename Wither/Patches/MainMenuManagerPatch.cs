using HarmonyLib;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(MainMenuManager))]
    public static class MainMenuManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(MainMenuManager.Start))]
        public static void Start(MainMenuManager __instance)
        {
            __instance.ShowAnnouncementPopUp();
        }
    }
}