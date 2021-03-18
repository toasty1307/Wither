using UnityEngine;
using Wither.Utils;

namespace Wither.Buttons
{
    public class ExplodeButton : Button
    {
        public ExplodeButton(Vector2 _offset, float cooldown) : base(_offset, "ExplodeImage", cooldown) { }

        protected override void OnClick()
        {
            
        }

        protected override bool CanUse()
        {
            return base.CanUse() && PlayerControl.LocalPlayer.Data.IsImpostor;
        }
    }
}