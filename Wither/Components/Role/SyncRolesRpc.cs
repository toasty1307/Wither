using System.Collections.Generic;
using Hazel;
using Reactor;
using Reactor.Networking;

namespace Wither.Components.Role
{
    [RegisterCustomRpc((uint) CustomRpc.CustomRpc.SyncRoles)]
    public class SyncRolesRpc : PlayerCustomRpc<WitherPlugin , byte[]>
    {
        public SyncRolesRpc(WitherPlugin plugin, uint id) : base(plugin, id) { }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;

        public override void Write(MessageWriter writer, byte[] data)
        {
            foreach (byte b in data)
            {
                writer.Write(b);
            }
        }

        public override byte[] Read(MessageReader reader)
        {
            var data = new List<byte>();
            foreach (var role in Components.Role.Role.roles)
            {
                for (int i = 0; i < role.Count; i++)
                {
                    data.Add(reader.ReadByte());
                }
            }

            return data.ToArray();
        }

        public override void Handle(PlayerControl innerNetObject, byte[] data)
        {
            int j = 0;
            foreach (var role in Components.Role.Role.roles)
            {
                role.players.Clear();
                for (int i = 0; i < role.Count; i++)
                {
                    role.players.Add(GameData.Instance.GetPlayerById(data[j])._object);
                    j++;
                }
            }
        }
    }
}