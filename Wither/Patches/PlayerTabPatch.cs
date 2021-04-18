using HarmonyLib;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(PlayerTab))]
    public static class PlayerTabPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
        public static void PreOnEnable(PlayerTab __instance)
        {
            __instance.YRange.min = -Palette.PlayerColors.Length / 3;
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
        public static void PostOnEnable(PlayerTab __instance)
        {
            foreach (var colorChip in __instance.ColorChips)
            {
            }
        }
    }
}