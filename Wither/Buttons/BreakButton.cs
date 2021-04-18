using System.Collections.Generic;
using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.Components.Button;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Buttons
{
    public class BreakButton : Button
    {
        public static readonly List<Crack> cracks = new();

        protected override void OnClick()
        {
            Rpc<InstantiateCrackRpc>.Instance.Send(PlayerControl.LocalPlayer.transform.position);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(0.6169749f, 1.686336f);
            maxTimer = GameOptions.BreakCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>("BreakImage");

        }

        protected override bool CouldUse() =>
            PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse()
        {
            return true;
        }

        public static void InstantiateCrack(Vector2 position)
        {
            var crack = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>("Crack");
            var instantiate = Object.Instantiate(crack, ShipStatus.Instance.transform);
            instantiate.transform.position = position;
            instantiate.layer = 9;
            var coolCrack = instantiate.AddComponent<Crack>();
            coolCrack.id = Crack.GetAvailableId();
            cracks.Add(coolCrack);
        }
    }
}