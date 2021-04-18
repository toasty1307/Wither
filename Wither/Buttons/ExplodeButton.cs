using System.Linq;
using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.Components.Button;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.Utils;
using Object = UnityEngine.Object;

namespace Wither.Buttons
{
    public class ExplodeButton : Button
    {
        protected override void OnClick()
        {
            foreach (var pc in PlayerControl.AllPlayerControls.ToArray().ToList().Where(x => !x.Data.Disconnected && !x.Data.IsDead))
            {
                if (pc == null) continue;
                if (pc == PlayerControl.LocalPlayer) continue;
                if (!(Vector3.Distance(pc.GetTruePosition(), PlayerControl.LocalPlayer.GetTruePosition()) <= PlayerControl.GameOptions.KillDistance * GameOptions.ExplosionRadius)) continue;
                if (PhysicsHelpers.AnyNonTriggersBetween(PlayerControl.LocalPlayer.GetTruePosition(), pc.GetTruePosition(), PlayerControl.GameOptions.KillDistance * GameOptions.ExplosionRadius, Constants.ShipAndObjectsMask)) continue;
                PlayerControl.LocalPlayer.RpcMurderPlayer(pc);
            }

            Rpc<InstantiateExplosionRpc>.Instance.Send(PlayerControl.LocalPlayer.transform.position);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(1.716975f, 1.686336f);
            maxTimer = GameOptions.ExplodeCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>("ExplodeImage");
        }

        public static void InstantiateExplosion(Vector2 position)
        {
            var effect = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>("Explosion");
            var instantiate = Object.Instantiate(effect, position, Quaternion.identity);
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