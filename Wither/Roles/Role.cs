using System;
using System.Reflection;
using HarmonyLib;

namespace Wither.Roles
{
    public class Role
    {
        protected Role()
        {
            WitherPlugin.Logger.LogInfo("SOMEONEWANTSROLES?");
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CustomRole : Attribute
    {
        public static void CreateRoles()
        {
            Assembly assembly = typeof(WitherPlugin).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttribute(typeof(CustomRole)) == null || !type.IsSubclassOf(typeof(Role))) continue;
                type.GetConstructor(new Type[0])?.Invoke(new object[0]);
            }
        }
    }

    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class CreateRolePatch
    {
        public static void Postfix() => CustomRole.CreateRoles();
    }
}