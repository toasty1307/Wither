using Hazel;
using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.Buttons;

namespace Wither.CustomRpc
{
    [RegisterCustomRpc]
    public class InstantiateSkullRpc : PlayerCustomRpc<WitherPlugin, Vector2>
    {
        public InstantiateSkullRpc(WitherPlugin plugin) : base(plugin) { }
        public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;
        public override void Write(MessageWriter writer, Vector2 data) => writer.Write(data);
        public override Vector2 Read(MessageReader reader) => reader.ReadVector2();
        public override void Handle(PlayerControl innerNetObject, Vector2 data) => SkullButton.InstantiateSkull(data);
    }
}