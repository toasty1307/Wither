using Hazel;
using Reactor;
using Reactor.Networking;
using Wither.Buttons;

namespace Wither.CustomRpc
{
    [RegisterCustomRpc((uint)CustomRpc.DestroyCrack)]
    public class DestroyCrackRpc : PlayerCustomRpc<WitherPlugin, int>
    {
        public DestroyCrackRpc(WitherPlugin plugin, uint id) : base(plugin, id) { }
        public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;
        public override void Write(MessageWriter writer, int data) => writer.Write(data);
        public override int Read(MessageReader reader) => reader.ReadInt32();
        public override void Handle(PlayerControl innerNetObject, int data) => FixButton.Fix(data);
    }
}