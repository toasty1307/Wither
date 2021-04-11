using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace Wither.Components.Roles
{
    [HarmonyPatch(typeof(PlayerControl))]
    public static class PlayerControlSetInfPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControl.RpcSetInfected))]
        public static void SetInfected() => Role.SetInfected();
        
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
        public static void FixedUpdate() => Role.PlayerControlFixedUpdate();
    }
    
    [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__11))]
    public static class IntroCutscenePatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(IntroCutscene._CoBegin_d__11.MoveNext))]
        public static void MoveNext(IntroCutscene._CoBegin_d__11 __instance) => __instance = Role.IntroCutscene(__instance);
    }
    
    [HarmonyPatch(typeof(IntroCutscene))]
    public static class IntroCutsceneCoBeginPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(IntroCutscene.CoBegin))]
        public static void CoBegin([HarmonyArgument(0)] ref List<PlayerControl> team)
        {
            team = Role.IntroCutsceneCoBegin(team);
        }
    }
    
    [HarmonyPatch(typeof(GameData))]
    public static class ComputeTasksPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GameData.RecomputeTaskCounts))]
        public static bool RecomputeTaskCounts() => Role.ComputeTasks();
    }
}