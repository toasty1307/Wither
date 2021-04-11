using System;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using Reactor;
using UnityEngine.SceneManagement;
using Wither.Components.Buttons;
using Wither.Components.Option;
using Wither.Components.Roles;
using Wither.CustomGameOptions;
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
        
        public static ManualLogSource Logger { get; private set; }

        public static WitherPlugin Instance;
        
        public override void Load()
        {
            Instance = this;
            
            Logger = Log;
            
            RegisterCustomRpcAttribute.Register(this);

            RegisterInIl2CppAttribute.Register();
            
            Harmony.PatchAll();
            
            AssetBundleLoader.LoadBundles();
            
            SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>) ((s, _) =>
            {
                if (s.buildIndex == 0) SceneManager.LoadScene(1);
            }));
            
            Button.CreateButtons();
            
            Role.CreateRoles();
            
            GameOptions.CreateOptions(); 
            
            CustomOption.UpdateAll();
        }
    }
}