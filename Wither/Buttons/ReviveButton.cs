using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.Utils;

namespace Wither.Components.Buttons
{
    [CustomButton]
    public class ReviveButton : Button
    {
        private static int Lives = CustomGameOptions.GameOptions.CrewLives;

        protected override void OnClick()
        {
            Rpc<ReviveRpc>.Instance.Send(PlayerControl.LocalPlayer.PlayerId);
            Lives--;
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = Vector2.zero;
            maxTimer = GameOptions.ReviveCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.ReviveImage);
        }

        protected override bool CouldUse()
        {
            return !PlayerControl.LocalPlayer.Data.IsImpostor && PlayerControl.LocalPlayer.Data.IsDead && Lives > 0;
        }

        protected override bool CanUse()
        {
            return true;
        }
    }
}