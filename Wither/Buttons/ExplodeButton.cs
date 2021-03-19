using UnityEngine;
using Wither.Utils;

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
                if (collider2D.gameObject.GetComponent<PlayerControl>() != null && collider2D.gameObject.GetComponent<PlayerControl>() != PlayerControl.LocalPlayer)
                    PlayerControl.LocalPlayer.RpcMurderPlayer(collider2D.gameObject.GetComponent<PlayerControl>());
            }
        }

        protected override bool CanUse()
        {
            return base.CanUse() && PlayerControl.LocalPlayer.Data.IsImpostor && GlobalVars.isTransformed;
        }
    }
}