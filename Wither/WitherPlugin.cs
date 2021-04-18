using System;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using UnityEngine.SceneManagement;
using Wither.Components.Button;
using Wither.Components.Role;
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

        public Harmony Harmony { get; } = new(Id);
        
        public override void Load()
        {
            PluginSingleton<WitherPlugin>.Instance = this;
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
            Colors.SetUpColors();
        }
    }
}