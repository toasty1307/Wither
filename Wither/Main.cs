using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using Reactor;
using UnityEngine;
using Wither.Utils;

namespace Wither
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class Main : BasePlugin
    {
        public const string Id = "me.toasty_marshmallow.Wither";

        public Harmony Harmony { get; } = new Harmony(Id);
        
        public static ManualLogSource Logger { get; private set; }
        
        public override void Load()
        {
            Logger = Log;

            RegisterCustomRpcAttribute.Register(this);

            RegisterInIl2CppAttribute.Register();
            
            Harmony.PatchAll();
            
            AssetBundleLoader.LoadBundles();
        }
    }
}