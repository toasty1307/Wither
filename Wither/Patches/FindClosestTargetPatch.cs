using HarmonyLib;
using UnityEngine;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FindClosestTarget))]
    public static class FindClosestTargetPatch
    {
        public static bool Prefix(PlayerControl __instance, ref PlayerControl __result)
        {
            PlayerControl result = null;
            float num = float.MaxValue;
            if (!ShipStatus.Instance)
            {
                __result = null;
            }
            Vector2 truePosition = __instance.GetTruePosition();
            Il2CppSystem.Collections.Generic.List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers;
            for (int i = 0; i < allPlayers.Count; i++)
            {
                GameData.PlayerInfo playerInfo = allPlayers[i];
                if (!playerInfo.Disconnected && playerInfo.PlayerId != __instance.PlayerId && !playerInfo.IsDead && !playerInfo.IsImpostor)
                {
                    PlayerControl @object = playerInfo.Object;
                    if (@object)
                    {
                        Vector2 vector = @object.GetTruePosition() - truePosition;
                        float magnitude = vector.magnitude;
                        if (magnitude <= num)
                        {
                            result = @object;
                            num = magnitude;
                        }
                    }
                }
            }
            __result = result;
            return false;
        }
    }
}