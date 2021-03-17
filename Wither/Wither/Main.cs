using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;

namespace Wither
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class Main : BasePlugin
    {
        public const string Id = "me.toasty_marshmallow.Wither";

        public Harmony Harmony { get; } = new Harmony(Id);
        
        public override void Load()
        {
            Harmony.PatchAll();
        }
    }
}