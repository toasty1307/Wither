using HarmonyLib;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.RpcEndGame))]
    public static class CheckEndPatch
    {
        public static bool Prefix([HarmonyArgument(0)] GameOverReason reason)
        {
            if (reason != GameOverReason.ImpostorBySabotage) return true;
            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, (byte)SystemTypes.Reactor);
            return false;
        }
    }
}