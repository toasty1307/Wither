using UnityEngine;
using Wither.Utils;

namespace Wither.Buttons
{
    public class BedrockButton : Button
    {
        public BedrockButton(Vector2 _offset, float cooldown) : base(_offset, "BedrockImage", cooldown) { }

        protected override void OnClick()
        {
            
        }

        protected override bool CanUse()
        {
            return base.CanUse() && !PlayerControl.LocalPlayer.Data.IsImpostor;
        }
    }
}