using Hazel;
using Reactor;
using Reactor.Networking;
using UnityEngine;

namespace Wither.CustomRpc
{
    [RegisterCustomRpc((uint)CustomRpc.Revive)]
    public class ReviveRpc : PlayerCustomRpc<WitherPlugin, byte>
    {
        public ReviveRpc(WitherPlugin plugin, uint id) : base(plugin, id) { }
        public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;
        public override void Write(MessageWriter writer, byte data) => writer.Write(data);
        public override byte Read(MessageReader reader) => reader.ReadByte();

        public override void Handle(PlayerControl innerNetObject, byte data)
        {
            GameData.Instance.GetPlayerById(data)._object.Revive();
        }
    }
}