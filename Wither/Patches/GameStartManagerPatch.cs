using HarmonyLib;
using UnityEngine;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
    public static class GameStartManagerPatch
    {
        public static void Postfix(GameStartManager __instance)
        {
            if (AmongUsClient.Instance.GameMode == GameModes.LocalGame ||AmongUsClient.Instance.GameMode == GameModes.FreePlay) return;
            __instance.GameRoomName.gameObject.SetActive(false);
            string code = __instance.GameRoomName.Text.Substring(6);
            GUIUtility.systemCopyBuffer = code;
        }
    }
}