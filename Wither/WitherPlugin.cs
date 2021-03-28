using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using Reactor;
using Wither.Utils;

namespace Wither
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class WitherPlugin : BasePlugin
    {
        public const string Id = "me.toasty_marshmallow.Wither";

        public Harmony Harmony { get; } = new Harmony(Id);

        public static WitherPlugin Instance;
        
        public static ManualLogSource Logger { get; private set; }
        
        public override void Load()
        {
            Logger = Log;

            Instance = this;

            RegisterCustomRpcAttribute.Register(this);

            RegisterInIl2CppAttribute.Register();
            
            Harmony.PatchAll();
            
            AssetBundleLoader.LoadBundles();
        }
    }
}