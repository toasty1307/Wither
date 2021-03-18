using UnityEngine;
using Wither.Utils;

namespace Wither.Buttons
{
    public class TransformButton : Button
    {
        public TransformButton(Vector2 _offset, float cooldown) : base(_offset, "TransformImage", cooldown) { }

        protected override void OnClick()
        {
            
        }

        protected override bool CanUse()
        {
            return base.CanUse() && PlayerControl.LocalPlayer.Data.IsImpostor;
        }
    }
}