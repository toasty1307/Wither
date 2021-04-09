using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace Wither.Components.Option
{
    [HarmonyPatch(typeof(GameOptionsMenu))]
    public static class GameOptionsMenuPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameOptionsMenu.Start))]
        public static void Start(GameOptionsMenu __instance)
        {
            CustomOption.menu = __instance;
            CustomOption.Prefabs();
            CustomOption.Create();
        }
    }
    
    [HarmonyPatch(typeof(NumberOption))]
    public static class NumberOptionPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(NumberOption.Increase))]
        public static bool Increase(NumberOption __instance)
        {
            return CustomNumberOption.Increase(__instance);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NumberOption.Decrease))]
        public static bool Decrease(NumberOption __instance)
        {
            return CustomNumberOption.Decrease(__instance);
        }
    }
    
    [HarmonyPatch(typeof(ToggleOption))]
    public static class ToggleOptionPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(ToggleOption.Toggle))]
        public static bool Toggle(ToggleOption __instance)
        {
            return CustomToggleOption.Toggle(__instance);
        }
    }

    [HarmonyPatch]
    public static class ToHudStringPatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            var bases = new List<MethodBase>();
            for (var index = 0; index < typeof(GameOptionsData).GetMethods().Length; index++)
            {
                var method = typeof(GameOptionsData).GetMethods()[index];
                if (method.ReturnType == typeof(string) && method.GetParameters().Length == 1)
                {
                    bases.Add(method);
                }
            }
            return bases.AsEnumerable();
        }
            
        public static void Postfix(ref string __result)
        {
            __result = CustomOption.ToString(__result);
        }
    }

    [HarmonyPatch(typeof(StringOption))]
    public static class StringOptionPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StringOption.Increase))]
        public static bool Increase(StringOption __instance)
        {
            return CustomStringOption.Increase(__instance);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StringOption.Decrease))]
        public static bool Decrease(StringOption __instance)
        {
            return CustomStringOption.Decrease(__instance);
        }
    }
    
    [HarmonyPatch(typeof(PlayerControl))]
    public static class PlayerControlPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControl.RpcSyncSettings))]
        public static void RpcSyncSettings()
        {
            CustomOption.SyncAll();
        }
    }

}