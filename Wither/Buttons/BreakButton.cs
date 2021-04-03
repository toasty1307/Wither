using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Components.Buttons
{
    [CustomButton]
    public class BreakButton : Button
    {

        protected override void OnClick()
        {
            Rpc<InstantiateCrackRpc>.Instance.Send(PlayerControl.LocalPlayer.transform.position);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = Vector2.up * 2;
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