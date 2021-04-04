using Hazel;
using Reactor;
using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.Components.Buttons;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.CustomRpc
{
    [RegisterCustomRpc((uint)CustomRpc.InstantiateRose)]
    public class InstantiateRoseRpc : PlayerCustomRpc<WitherPlugin, Vector2>
    {
        public InstantiateRoseRpc(WitherPlugin plugin, uint id) : base(plugin, id) { }
        public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;
        public override void Write(MessageWriter writer, Vector2 data) => writer.Write(data);
        public override Vector2 Read(MessageReader reader) => reader.ReadVector2();
        public override void Handle(PlayerControl innerNetObject, Vector2 data)
        {
            GameObject rose = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>(Utils.StringNames.WitherRose);
            GameObject instantiate = Object.Instantiate(rose, ShipStatus.Instance.transform);
            instantiate.transform.position = data;
            instantiate.AddComponent<WitherRose>();
        }
    }
}