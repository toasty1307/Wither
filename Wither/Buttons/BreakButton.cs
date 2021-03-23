using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.CustomRpc;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Buttons
{
    public class BreakButton : Button
    {
        public BreakButton(Vector2 _offset, float cooldown) : base(_offset, Utils.StringNames.BreakImage, cooldown) { }

        protected override void OnClick()
        {
            Rpc<InstantiateCrackRpc>.Instance.Send(new InstantiateCrackRpc.Data(PlayerControl.LocalPlayer.transform.position));
        }

        protected override bool CanUse()
        {
            return base.CanUse() && PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed;
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