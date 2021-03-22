using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.CustomRpc;
using Wither.Utils;
using Object = UnityEngine.Object;

namespace Wither.Buttons
{
    public class ExplodeButton : Button
    {
        public ExplodeButton(Vector2 _offset, float cooldown) : base(_offset, "ExplodeImage", cooldown) { }

        protected override void OnClick()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), CustomGameOptions.ExplosionRadius);
            foreach (var collider2D in colliders)
            {
                PlayerControl pc = collider2D.gameObject.GetComponent<PlayerControl>();
                if (pc != null &&  pc != PlayerControl.LocalPlayer && !pc.Data.IsDead)
                    PlayerControl.LocalPlayer.RpcMurderPlayer(collider2D.gameObject.GetComponent<PlayerControl>());
            }

            Rpc<InstantiateExplosionRpc>.Instance.Send(new InstantiateExplosionRpc.Data(PlayerControl.LocalPlayer.transform.position));
        }

        public static void InstantiateExplosion(Vector2 position)
        {
            GameObject effect = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>("Explosion");
            GameObject instantiate = Object.Instantiate(effect, PlayerControl.LocalPlayer.transform);
            Object.Destroy(instantiate, 5f);
        }

        protected override bool CanUse()
        {
            return base.CanUse() && PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed;
        }
    }
}