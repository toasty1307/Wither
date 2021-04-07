using UnityEngine;

namespace Wither.Components.Roles
{
    public class Mechanic : Role
    {
        public override string Name { get; } = "Mechanic";
        public override int Count { get; } = 1;
        public override bool ShowTeam { get; } = true;
        public override bool ShowRole { get; } = false;
        public override Color32 Color { get; } = new Color32(89, 243, 255, 255);
        public override RoleSide RoleSide { get; } = RoleSide.Crewmate;
        public override string Description { get; } = "Do whatever you want lol";
        public override bool HasTasks { get; } = false;
    }
}