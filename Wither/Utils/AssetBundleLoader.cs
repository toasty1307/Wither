using System.IO;
using System.Reflection;
using Reactor.Extensions;
using UnityEngine;

namespace Wither.Utils
{
    public static class AssetBundleLoader
    {
        public static AssetBundle ButtonTextureBundle;

        public static AssetBundle PrefabBundle;
        
        public static void LoadBundles()
        {
            Assembly executingAssembly = typeof(WitherPlugin).Assembly;
            Stream stream;
            
            
            // ButtonTextureBundle
            using (stream = executingAssembly.GetManifestResourceStream("Wither.AssetBundles." + StringNames.ButtonTextureAssetBundle))
                ButtonTextureBundle = AssetBundle.LoadFromMemory(stream.ReadFully());
            // ButtonTextureBundle
            
            // PrefabBundle
            using (stream = executingAssembly.GetManifestResourceStream("Wither.AssetBundles." + StringNames.PrefabAssetBundle))
                PrefabBundle = AssetBundle.LoadFromMemory(stream.ReadFully());
            // PrefabBundle
        }
    }
}