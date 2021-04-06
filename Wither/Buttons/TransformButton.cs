using Reactor.Extensions;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.Utils;

namespace Wither.Components.Buttons
{
    public class TransformButton : Button
    {        
        public static bool isTransformed = false;

        protected override void OnClick()
        {
            isTransformed = true;
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(0.6169749f, 0.5863363f);
            maxTimer = GameOptions.TransformCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.TransformImage);
        }

        protected override bool CouldUse() => PlayerControl.LocalPlayer.Data.IsImpostor && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse() => !isTransformed;
    }
}