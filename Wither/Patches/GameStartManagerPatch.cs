using HarmonyLib;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
    public static class GameStartManagerPatch
    {
        public static void Postfix()
        {
            
        }
    }
}