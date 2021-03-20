using UnityEngine;
using Wither.Utils;

namespace Wither.Buttons
{
    public class BreakButton : Button
    {
        public BreakButton(Vector2 _offset, float cooldown) : base(_offset, "BreakImage", cooldown) { }

        protected override void OnClick()
        {
            
        }

        protected override bool CanUse()
        {
            return base.CanUse() && PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed;
        }
    }
}