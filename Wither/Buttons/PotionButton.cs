using System.Collections;
using System.Collections.Generic;
using Reactor;
using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.Components.Button;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.Utils;

namespace Wither.Buttons
{
    public class PotionButton : Button
    {
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        private static readonly int Outline = Shader.PropertyToID("_Outline");

        public static List<PlayerControl> strongBoi = new();
        protected override bool CouldUse() => !PlayerControl.LocalPlayer.Data.IsImpostor && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse() => true;

        protected override void OnClick()
        {
            Rpc<GlowRpc>.Instance.Send(PlayerControl.LocalPlayer.PlayerId);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(0.6169749f, 1.686336f);
            maxTimer = GameOptions.PotionCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>("MilkImage");
            hasLimitedUse = true;
            maxUses = GameOptions.PotionUses;
        }

        public static void GlowPlayer(PlayerControl player)
        {
            player.myRend.material.SetFloat(Outline, 1);
            player.myRend.material.SetColor(OutlineColor, Color.green);
            strongBoi.Add(player);
            Coroutines.Start(StopGlowing(player));
        }

        public static IEnumerator StopGlowing(PlayerControl player)
        {
            yield return new WaitForSecondsRealtime(GameOptions.PotionEffectCooldown);
            player.myRend.material.SetFloat(Outline, 0);
            player.myRend.material.SetColor(OutlineColor, Color.clear);
            strongBoi.Remove(player);
        }
    }
}