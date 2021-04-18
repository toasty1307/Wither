using HarmonyLib;

namespace Wither.Components.Cosmetics
{
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetHatById))]
        public static bool Awake()
        {
            return Hats.RegisterAll();
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        public static void FixedUpdate(PlayerControl __instance)
        {
            Hats.FixedUpdate(__instance);
        }
    }
}