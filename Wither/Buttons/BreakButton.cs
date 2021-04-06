using Reactor;
using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Components.Buttons
{
    public class BreakButton : Button
    {

        protected override void OnClick()
        {
            Rpc<InstantiateCrackRpc>.Instance.Send(PlayerControl.LocalPlayer.transform.position);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(0.6169749f, 1.686336f);
            maxTimer = GameOptions.BreakCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.BreakImage);

        }

        protected override bool CouldUse() =>
            PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse()
        {
            return true;
        }

        public static void InstantiateCrack(Vector2 position)
        {
            GameObject crack = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>(Utils.StringNames.Crack);
            GameObject instantiate = Object.Instantiate(crack, ShipStatus.Instance.transform);
            instantiate.transform.position = position;
            instantiate.layer = 9;
            instantiate.AddComponent<Crack>();
        }
    }
}