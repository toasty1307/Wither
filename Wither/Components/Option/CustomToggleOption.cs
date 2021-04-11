using System;
using BepInEx.Configuration;
using Reactor.Networking;
using Rewired.Data.Mapping;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wither.Components.Option
{
    public class CustomToggleOption : CustomOption
    {
        public readonly ConfigEntry<bool> ConfigEntry;
        public event EventHandler<OnValueChangedEventArgs> OnValueChanged;
        
        public new ToggleOption Data
        {
            get => (ToggleOption) base.Data;
            set => base.Data = value;
        }

        public new bool Value
        {
            get => (bool) base.Value;
            set => base.Value = value;
        }

        public CustomToggleOption(string id, string name, bool value, bool showValue = true) : base(id, name, OptionType.Toggle, value, showValue)
        {
            ConfigEntry = WitherPlugin.Instance.Config.Bind("Custom Game Options", id, Value);
            Value = ConfigEntry.Value;
            OnValueChanged += (sender, args) => { ConfigEntry.Value = (bool) args.Value; Value = (bool) args.Value; };
        }
        
        public CustomToggleOption(string id, string name, bool value, Color color, bool showValue = true) : base(id, name, OptionType.Toggle, value, color, showValue)
        {
            ConfigEntry = WitherPlugin.Instance.Config.Bind("Custom Game Options", id, Value);
            Value = ConfigEntry.Value;
            OnValueChanged += (sender, args) => { ConfigEntry.Value = (bool) args.Value; Value = (bool) args.Value; };
        }

        public override void CreateOption()
        {
            Data = Object.Instantiate(ToggleOptionPrefab.gameObject, menu.transform).GetComponent<ToggleOption>();
            Data.TitleText.Text = Name;
            Data.CheckMark.enabled = ConfigEntry.Value;
            Position(Data.transform);
        }

        public static bool Toggle(ToggleOption option)
        {
            foreach (var customOption in Options)
            {
                if (customOption is CustomToggleOption toggleOption)
                {
                    if (toggleOption.Data == option)
                    {
                        option.CheckMark.enabled = toggleOption.Value = !toggleOption.Value;
                        toggleOption.RaiseOnValueChanged(option.CheckMark.enabled);
                        PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                        return false;
                    }
                }
            }

            return true;
        }

        public void RaiseOnValueChanged(bool value)
        {
            OnValueChanged?.Invoke(null, new OnValueChangedEventArgs(value));
        }
    }
}