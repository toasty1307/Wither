using HarmonyLib;
using UnityEngine;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(Vent))]
    public static class VentPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Vent.CanUse))]
        public static bool CanUse(Vent __instance, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse, ref float __result)
        {
            float num = float.MaxValue;
            PlayerControl @object = pc.Object;
            couldUse = (!pc.IsImpostor && !pc.IsDead && (@object.CanMove || @object.inVent));
            canUse = couldUse;
            if (canUse)
            {
                Vector2 truePosition = @object.GetTruePosition();
                Vector3 position = __instance.transform.position;
                num = Vector2.Distance(truePosition, position);
                canUse &= num <= __instance.UsableDistance && !PhysicsHelpers.AnythingBetween(truePosition, position, Constants.ShipOnlyMask, false);
            }
            __result = num;
            return !CustomGameOptions.GameOptions.CrewCanVent;
        }
    }
}