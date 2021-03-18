using UnityEngine;
using Wither.Utils;

namespace Wither.Buttons
{
    public class SkullButton : Button
    {
        public SkullButton(Vector2 _offset, float cooldown) : base(_offset, "SkullImage", cooldown) { }

        protected override void OnClick()
        {
            
        }

        protected override bool CanUse()
        {
            return base.CanUse() && PlayerControl.LocalPlayer.Data.IsImpostor;
        }
    }
}