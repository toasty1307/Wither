using System.Linq;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace Wither.Components.Roles
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
    public static class PlayerControlSetInfPatch
    {
        public static void Postfix() => Role.SetInfected();
    }
    
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class PlayerControlFUpdatePatch
    {
        public static void Postfix() => Role.PlayerControlFixedUpdate();
    }
    
    [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__11), nameof(IntroCutscene._CoBegin_d__11.MoveNext))]
    public static class IntroCutscenePatch
    {
        public static void Postfix(IntroCutscene._CoBegin_d__11 __instance) => __instance = Role.IntroCutscene(__instance);
    }
    
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CoBegin))]
    public static class IntroCutsceneCoBeginPatch
    {
        public static void Prefix([HarmonyArgument(0)] ref List<PlayerControl> team)
        {
            team = Role.IntroCutsceneCoBegin(team);
        }
    }
    
    [HarmonyPatch(typeof(GameData), nameof(GameData.RecomputeTaskCounts))]
    public static class ComputeTasksPatch
    {
        public static bool Prefix() => Role.ComputeTasks();
    }
}