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
        
        public static AssetBundle HatsBundle;
        
        public static void LoadBundles()
        {
            var executingAssembly = typeof(WitherPlugin).Assembly;
            Stream stream;
            
            
            // ButtonTextureBundle
            using (stream = executingAssembly.GetManifestResourceStream("Wither.AssetBundles.buttontextures"))
                ButtonTextureBundle = AssetBundle.LoadFromMemory(stream.ReadFully());
            // ButtonTextureBundle
            
            // PrefabBundle
            using (stream = executingAssembly.GetManifestResourceStream("Wither.AssetBundles.prefabs"))
                PrefabBundle = AssetBundle.LoadFromMemory(stream.ReadFully());
            // PrefabBundle
            
            // PrefabBundle
            using (stream = executingAssembly.GetManifestResourceStream("Wither.AssetBundles.hats"))
                HatsBundle = AssetBundle.LoadFromMemory(stream.ReadFully());
            // PrefabBundle
        }
    }
}