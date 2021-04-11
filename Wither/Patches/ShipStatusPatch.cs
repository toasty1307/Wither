using HarmonyLib;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(ShipStatus))]
    public static class ShipStatusPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.RpcEndGame))]
        public static bool RpcEndGame([HarmonyArgument(0)] GameOverReason reason)
        {
            if (reason != GameOverReason.ImpostorBySabotage) return true;
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, (byte)SystemTypes.Reactor);
            return false;
        }
    }
}