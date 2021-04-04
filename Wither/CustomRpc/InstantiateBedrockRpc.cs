using Hazel;
using Reactor;
using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.Components.Buttons;

namespace Wither.CustomRpc
{
    [RegisterCustomRpc((uint)CustomRpc.InstantiateBedrock)]
    public class InstantiateBedrockRpc : PlayerCustomRpc<WitherPlugin, Vector2>
    {
        public InstantiateBedrockRpc(WitherPlugin plugin, uint id) : base(plugin, id) { }
        public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;
        public override void Write(MessageWriter writer, Vector2 data) => writer.Write(data);
        public override Vector2 Read(MessageReader reader) => reader.ReadVector2();
        public override void Handle(PlayerControl innerNetObject, Vector2 data) => BedrockButton.InstantiateBedrock(data);
    }
}