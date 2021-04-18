using UnityEngine;
using Wither.Components.Role;

namespace Wither.Roles
{
    public class Mechanic : Role
    {
        public override string Name => "Mechanic";
        public override int Count => 1;
        public override bool ShowTeam => true;
        public override bool ShowRole => true;
        public override Color32 Color => new(89, 243, 255, 255);
        public override RoleSide RoleSide => RoleSide.Crewmate;
        public override string Description => "Do whatever you want lol";
        public override bool HasTasks => true;
    }
}