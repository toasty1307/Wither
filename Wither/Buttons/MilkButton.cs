using Reactor.Extensions;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.Utils;

namespace Wither.Buttons
{
    public class MilkButton : Button
    {        
        protected override void OnClick()
        {
        }

        protected override void SetVars()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = Vector2.up * 2;
            maxTimer = GameOptions.MilkCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.MilkImage);
        }

        protected override bool CouldUse() => !PlayerControl.LocalPlayer.Data.IsImpostor && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse() => true;
    }
}