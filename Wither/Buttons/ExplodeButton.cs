using System.Linq;
using System.Runtime.CompilerServices;
using Reactor;
using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.Utils;
using Object = UnityEngine.Object;

namespace Wither.Components.Buttons
{
    [CustomButton]
    public class ExplodeButton : Button
    {
        protected override void OnClick()
        {
            foreach (var pc in PlayerControl.AllPlayerControls.ToArray().ToList().Where(x => !x.Data.Disconnected && !x.Data.IsDead))
            {
                if (pc == null) continue;
                Vector2 vector = pc.GetTruePosition() - PlayerControl.LocalPlayer.GetTruePosition();
                float magnitude = vector.magnitude;
                if (pc != PlayerControl.LocalPlayer && !pc.Data.IsDead
                    && !PhysicsHelpers.AnyNonTriggersBetween(PlayerControl.LocalPlayer.GetTruePosition(), vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                    PlayerControl.LocalPlayer.RpcMurderPlayer(pc);
            }

            Rpc<InstantiateExplosionRpc>.Instance.Send(PlayerControl.LocalPlayer.transform.position);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = Vector2.one * 2;
            maxTimer = GameOptions.ExplodeCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.ExplodeImage);
        }

        public static void InstantiateExplosion(Vector2 position)
        {
            GameObject effect = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>(Utils.StringNames.Explosion);
            GameObject instantiate = Object.Instantiate(effect, position, Quaternion.identity);
            Object.Destroy(instantiate, 5f);
        }

        protected override bool CouldUse() =>
            PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse()
        {
            return true;
        }
    }
}