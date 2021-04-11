using System;
using BepInEx.Configuration;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wither.Components.Option
{
    public class CustomNumberOption : CustomOption
    {
        public readonly float Increment;
        public readonly float Min;
        public readonly float Max;
        public readonly string Format;
        public readonly ConfigEntry<float> ConfigEntry;
        public event EventHandler<OnValueChangedEventArgs> OnValueChanged;
        
        public new NumberOption Data
        {
            get => (NumberOption) base.Data;
            set => base.Data = value;
        }

        public new float Value
        {
            get => (float) base.Value;
            set => base.Value = value;
        }

        public CustomNumberOption(string id, string name, float value, float increment = 0.25f, float min = 0.25f, float max = 10f, string format = "0.0", bool showValue = true) : base(id, name, OptionType.Number, value, showValue)
        {
            Increment = increment;
            Min = Mathf.Min(value, min);
            Max = Mathf.Max(value, max);
            Format = format;
            ConfigEntry = WitherPlugin.Instance.Config.Bind("Custom Game Options", Id, Value);
            ConfigEntry.Value = Mathf.Clamp(ConfigEntry.Value, Min, Max);
            Value = ConfigEntry.Value;
            OnValueChanged += (sender, args) => { ConfigEntry.Value = (float) args.Value; Value = (float) args.Value; };
        }
        
        public CustomNumberOption(string id, string name, float value, Color color, float increment = 0.25f, float min = 0.25f, float max = 10f, string format = "0.0", bool showValue = true) : base(id, name, OptionType.Number, value, color, showValue)
        {
            Increment = increment;
            Min = Mathf.Min(value, min);
            Max = Mathf.Max(value, max);
            Format = format;
            ConfigEntry = WitherPlugin.Instance.Config.Bind("Custom Game Options", Id, Value);
            ConfigEntry.Value = Mathf.Clamp(ConfigEntry.Value, Min, Max);
            Value = ConfigEntry.Value;
            OnValueChanged += (sender, args) => { ConfigEntry.Value = (float) args.Value; Value = (float) args.Value; };
        }

        public override void CreateOption()
        {
            Data = Object.Instantiate(NumberOptionPrefab.gameObject, menu.transform).GetComponent<NumberOption>();
            Data.TitleText.Text = Name;
            Data.Increment = Increment;
            Data.ValidRange = new FloatRange(Min, Max);
            Data.FormatString = Format;
            Data.Value = ConfigEntry.Value;
            Position(Data.transform);
        }

        public static bool Increase(NumberOption option)
        {
            foreach (var customOption in Options)
            {
                if (customOption is CustomNumberOption numberOption)
                {
                    if (numberOption.Data == option)
                    {
                        option.Value = numberOption.Value = Mathf.Min(numberOption.Value + numberOption.Increment, numberOption.Max);
                        numberOption.RaiseOnValueChanged(option.Value);
                        PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                        return false;
                    }
                }
            }

            return true;
        }
        
        public static bool Decrease(NumberOption option)
        {
            foreach (var customOption in Options)
            {
                if (customOption is CustomNumberOption numberOption)
                {
                    if (numberOption.Data == option)
                    {
                        option.Value = numberOption.Value = Mathf.Max(numberOption.Value - numberOption.Increment, numberOption.Min);
                        numberOption.RaiseOnValueChanged(option.Value);
                        PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                        return false;
                    }
                }
            }

            return true;
        }

        public void RaiseOnValueChanged(float value)
        {
            OnValueChanged?.Invoke(null, new OnValueChangedEventArgs(value));
        }
    }
}