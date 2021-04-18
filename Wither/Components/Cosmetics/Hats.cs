using System.Collections.Generic;
using Reactor.Extensions;
using UnityEngine;
using Wither.Utils;

namespace Wither.Components.Cosmetics
{
    public class Hats
    {
        public static readonly List<HatData> CustomHats = new()
        {
            new HatData("CoolHat", true, new Vector2(-0.1f, -50f), AssetBundleLoader.HatsBundle.LoadAsset<Sprite>("CoolHat"), "not me")
        };
        
        private static int Id = HatManager.Instance.AllHats.Count;
        
        public static void Register(HatData data)
        {
            var hat = ScriptableObject.CreateInstance<HatBehaviour>();
            hat.MainImage = data.Sprite;
            hat.ProductId = data.Name;
            hat.Order = Id++;
            hat.InFront = true;
            hat.NoBounce = !data.Bounce;
            hat.ChipOffset = data.Pivot;
            HatManager.Instance.AllHats.Add(hat);
        }

        private static bool added;

        public static bool RegisterAll()
        {
            if (added) return true;
            added = true;
            foreach (var customHat in CustomHats)
            {
                Register(customHat);
            }

            return true;
        }

        public static void FixedUpdate(PlayerControl player)
        {
            player.HatRenderer.transform.localPosition = HatManager.Instance.GetHatById(player.Data.HatId).ChipOffset;
        }
    }
    
    public readonly struct HatData
    {
        public readonly string Name;
        public readonly bool Bounce;
        public readonly Vector2 Pivot;
        public readonly Sprite Sprite;
        public readonly string Author;

        public HatData(string name, bool bounce, Vector2 pivot, Sprite sprite, string author)
        {
            Name = name;
            Bounce = bounce;
            Pivot = pivot;
            Sprite = sprite;
            Author = author;
        }
    }
}