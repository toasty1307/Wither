using UnityEngine;
using Wither.Utils;

namespace Wither.Buttons
{
    public class TransformButton : Button
    {
        public TransformButton(Vector2 _offset, float cooldown) : base(_offset, Utils.StringNames.TransformImage, cooldown) { }
        
        public static bool isTransformed = false;

        protected override void OnClick()
        {
            isTransformed = true;
        }

        protected override bool CanUse()
        {
            return base.CanUse() && PlayerControl.LocalPlayer.Data.IsImpostor;
        }
    }
}