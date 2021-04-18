using Hazel;
using Reactor;
using Reactor.Networking;
using Wither.Buttons;

namespace Wither.CustomRpc
{
    [RegisterCustomRpc((uint)CustomRpc.Glow)]
    public class GlowRpc : PlayerCustomRpc<WitherPlugin, byte>
    {
        public GlowRpc(WitherPlugin plugin, uint id) : base(plugin, id)
        {
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;

        public override void Write(MessageWriter writer, byte data)
        {
            writer.Write(data);
        }

        public override byte Read(MessageReader reader)
        {
            return reader.ReadByte();
        }

        public override void Handle(PlayerControl innerNetObject, byte data)
        {
            PotionButton.GlowPlayer(GameData.Instance.GetPlayerById(data)._object);
        }
    }
}