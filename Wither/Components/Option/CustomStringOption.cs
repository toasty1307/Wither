using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using Reactor;
using Reactor.Networking;
using UnhollowerBaseLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wither.Components.Option
{
    public class CustomStringOption : CustomOption
    {
        public readonly List<string> Values;
        public readonly ConfigEntry<byte> ConfigEntry;
        public event EventHandler<OnValueChangedEventArgs> OnValueChanged;
        public new StringOption Data
        {
            get => (StringOption) base.Data;
            set => base.Data = value;
        }

        public new string Value
        {
            get => (string) base.Value;
            set => base.Value = value;
        }
        
        public byte ByteValue
        {
            get => (byte) Values.IndexOf(Value);
            set => Value = Values[value];
        }
        
        public CustomStringOption(string id, string name, int value, string[] values, bool showValue = true) : base(id, name, OptionType.String, value, showValue)
        {
            Values = values.ToList();
            ByteValue = (byte) value;
            ConfigEntry = WitherPlugin.Instance.Config.Bind("Custom Game Options", Id, ByteValue);
            ConfigEntry.Value = (byte) Mathf.Clamp(ConfigEntry.Value, 0, Values.Count - 1);
            ByteValue = ConfigEntry.Value;
            OnValueChanged += (sender, args) => { ConfigEntry.Value = (byte) args.Value; ByteValue = ConfigEntry.Value; };
        }
        
        public override void CreateOption()
        {
            Data = Object.Instantiate(StringOptionPrefab.gameObject, menu.transform).GetComponent<StringOption>();
            Data.TitleText.Text = Name; 
            Data.Values = new Il2CppStructArray<StringNames>(Values.Count);
            Data.Value = ByteValue;
            for (int i = 0; i < Values.Count; i++)
            {
                Data.Values[i] = CustomStringName.Register(Values[i]);
            }
            Position(Data.transform);
        }

        public static bool Increase(StringOption option)
        {
            foreach (var customOption in Options)
            {
                if (!(customOption is CustomStringOption customStringOption)) continue;
                if (customStringOption.Data != option) continue;
                option.Value = Mathf.Clamp(option.Value + 1, 0, option.Values.Length - 1);
                customStringOption.ByteValue = (byte) option.Value;
                WitherPlugin.Logger.LogInfo(customStringOption.ByteValue);
                customStringOption.OnValueChanged.Invoke(customStringOption, new OnValueChangedEventArgs(customStringOption.ByteValue));
                Rpc<SyncSettingsRpc>.Instance.Send(new SyncSettingsRpc.Data(customStringOption.Id, (byte) customStringOption.Type, customStringOption.ByteValue));
                return false;
            }
            return true;
        }
        
        public static bool Decrease(StringOption option)
        {
            foreach (var customOption in Options)
            {
                if (!(customOption is CustomStringOption customStringOption)) continue;
                if (customStringOption.Data != option) continue;
                option.Value = Mathf.Clamp(option.Value - 1, 0, option.Values.Length - 1);
                customStringOption.ByteValue = (byte) option.Value;
                customStringOption.OnValueChanged.Invoke(customOption, new OnValueChangedEventArgs(customStringOption.ByteValue));
                Rpc<SyncSettingsRpc>.Instance.Send(new SyncSettingsRpc.Data(customStringOption.Id, (byte) customStringOption.Type, customStringOption.ByteValue));
                return false;
            }
            return true;
        }

        public void RaiseOnValueChanged(byte value)
        {
            OnValueChanged!.Invoke(null, new OnValueChangedEventArgs(value));
        }
    }
}