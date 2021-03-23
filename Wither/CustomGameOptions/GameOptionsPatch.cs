using HarmonyLib;
using System;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;
using Wither.CustomGameOptions;

namespace ReactorSheriff
{
    [HarmonyPatch(typeof(GameOptionsMenu))]
    public static class GameOptionsMenuPatch
    {
        public static ToggleOption DestroyBedrock;
        public static NumberOption TransformCooldown;
        public static NumberOption ExplodeCooldown;
        public static NumberOption BreakCooldown;
        public static NumberOption BedrockCooldown;
        public static NumberOption SkullCooldown;
        public static NumberOption ReviveCooldown;
        public static NumberOption ExplosionRadius;
        public static NumberOption WitherDeathTime;
        public static NumberOption BedrockDestroyTime;
        public static NumberOption WitherSkullSpeedMultiplier;
        public static NumberOption CrewLives;
        public static float LowestY;

        static float GetLowestConfigY(GameOptionsMenu __instance)
        {
            return __instance.GetComponentsInChildren<OptionBehaviour>()
                .Min(option => option.transform.localPosition.y);
        }

        public static void PositionElement(OptionBehaviour element)
        {
            LowestY -= 0.5f;
            element.transform.localPosition = new Vector3(element.transform.localPosition.x, LowestY,
                element.transform.localPosition.z);
        }

        public static ToggleOption PrepareToggle(GameOptionsMenu instance, string title, bool enabled)
        {
            ToggleOption toggle = UnityEngine.Object.Instantiate(instance.GetComponentsInChildren<ToggleOption>().Last(),
                instance.transform);
            PositionElement(toggle);
            toggle.TitleText.Text = title;
            toggle.CheckMark.enabled = enabled;

            return toggle;
        }

        public static NumberOption PrepareNumberOption(GameOptionsMenu instance, string title, float value)
        {
            NumberOption option = UnityEngine.Object.Instantiate(instance.GetComponentsInChildren<NumberOption>().Last(),
                instance.transform);

            PositionElement(option);
            option.gameObject.name = title;
            option.TitleText.Text = title;
            option.Value = value;
            option.ValueText.Text = value.ToString();

            return option;
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameOptionsMenu.Start))]
        public static void Postfix1(GameOptionsMenu __instance)
        {
            if (GameObject.FindObjectsOfType<ToggleOption>().Count == 4)
            {

                LowestY = GetLowestConfigY(__instance);
                
                System.Collections.Generic.List<OptionBehaviour> NewOptions = __instance.Children.ToList();

                DestroyBedrock = PrepareToggle(__instance, "Destroy Bedrock", GameOptions.DestroyBedrock);
                TransformCooldown = PrepareNumberOption(__instance, "Transform Cooldown", GameOptions.TransformCooldown);
                ExplodeCooldown = PrepareNumberOption(__instance, "Explode Cooldown", GameOptions.ExplodeCooldown);
                BreakCooldown = PrepareNumberOption(__instance, "Break Cooldown", GameOptions.BreakCooldown);
                BedrockCooldown = PrepareNumberOption(__instance, "Bedrock Cooldown", GameOptions.BedrockCooldown);
                SkullCooldown = PrepareNumberOption(__instance, "Skull Cooldown", GameOptions.SkullCooldown);
                ReviveCooldown = PrepareNumberOption(__instance, "Revive Cooldown", GameOptions.ReviveCooldown);
                ExplosionRadius = PrepareNumberOption(__instance, "ExplosionRadius Radius", GameOptions.ExplosionRadius);
                WitherDeathTime = PrepareNumberOption(__instance, "Wither Death Time", GameOptions.WitherDeathTime);
                BedrockDestroyTime = PrepareNumberOption(__instance, "Bedrock Destroy Time", GameOptions.BedrockDestroyTime);
                WitherSkullSpeedMultiplier = PrepareNumberOption(__instance, "WitherSkullSpeedMultiplier", GameOptions.WitherSkullSpeedMultiplier);
                CrewLives = PrepareNumberOption(__instance, "Crewmate Lives", GameOptions.CrewLives);
                
                NewOptions.Add(DestroyBedrock);
                NewOptions.Add(TransformCooldown);
                NewOptions.Add(ExplodeCooldown);
                NewOptions.Add(BreakCooldown);
                NewOptions.Add(BedrockCooldown);
                NewOptions.Add(SkullCooldown);
                NewOptions.Add(ReviveCooldown);
                NewOptions.Add(ExplosionRadius);
                NewOptions.Add(WitherDeathTime);
                NewOptions.Add(BedrockDestroyTime);
                NewOptions.Add(WitherSkullSpeedMultiplier);
                NewOptions.Add(CrewLives);
                
                __instance.GetComponentInParent<Scroller>().YBounds.max +=
                    0.5f * (NewOptions.Count - __instance.Children.Count);

                __instance.Children = new Il2CppReferenceArray<OptionBehaviour>(NewOptions.ToArray());
            }
        }
    }
    
    [HarmonyPatch(typeof(ToggleOption))]
    public static class ToggleButtonPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(ToggleOption.Toggle))]
        public static bool Prefix(ToggleOption __instance)
        {
            if (__instance.TitleText.Text == GameOptionsMenuPatch.DestroyBedrock.TitleText.Text)
            {
                GameOptions.DestroyBedrock = !GameOptions.DestroyBedrock;
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);

                __instance.oldValue = GameOptions.DestroyBedrock;
                __instance.CheckMark.enabled = GameOptions.DestroyBedrock;
                
                return false;
            }
            
            return true;
        }

    }
    
    [HarmonyPatch(typeof(NumberOption))]
    public static class NumberOptionPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(NumberOption.Increase))]
        public static bool Prefix1(NumberOption __instance)
        {
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.TransformCooldown.TitleText.Text)
            {
                GameOptions.TransformCooldown = Math.Min(GameOptions.TransformCooldown + 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.TransformCooldown;
                __instance.Value = GameOptions.TransformCooldown;
                __instance.ValueText.Text = GameOptions.TransformCooldown.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.ExplodeCooldown.TitleText.Text)
            {
                GameOptions.ExplodeCooldown = Math.Min(GameOptions.ExplodeCooldown + 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.ExplodeCooldown;
                __instance.Value = GameOptions.ExplodeCooldown;
                __instance.ValueText.Text = GameOptions.ExplodeCooldown.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.BedrockCooldown.TitleText.Text)
            {
                GameOptions.BedrockCooldown = Math.Min(GameOptions.BedrockCooldown + 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.BedrockCooldown;
                __instance.Value = GameOptions.BedrockCooldown;
                __instance.ValueText.Text = GameOptions.BedrockCooldown.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.ReviveCooldown.TitleText.Text)
            {
                GameOptions.ReviveCooldown = Math.Min(GameOptions.ReviveCooldown + 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.ReviveCooldown;
                __instance.Value = GameOptions.ReviveCooldown;
                __instance.ValueText.Text = GameOptions.ReviveCooldown.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.BreakCooldown.TitleText.Text)
            {
                GameOptions.BreakCooldown = Math.Min(GameOptions.BreakCooldown + 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.BreakCooldown;
                __instance.Value = GameOptions.BreakCooldown;
                __instance.ValueText.Text = GameOptions.BreakCooldown.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.WitherDeathTime.TitleText.Text)
            {
                GameOptions.WitherDeathTime = Math.Min(GameOptions.WitherDeathTime + 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.WitherDeathTime;
                __instance.Value = GameOptions.WitherDeathTime;
                __instance.ValueText.Text = GameOptions.WitherDeathTime.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.WitherSkullSpeedMultiplier.TitleText.Text)
            {
                GameOptions.WitherSkullSpeedMultiplier = Math.Min(GameOptions.WitherSkullSpeedMultiplier + 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.WitherSkullSpeedMultiplier;
                __instance.Value = GameOptions.WitherSkullSpeedMultiplier;
                __instance.ValueText.Text = GameOptions.WitherSkullSpeedMultiplier.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.CrewLives.TitleText.Text)
            {
                GameOptions.CrewLives = (int) Math.Min(GameOptions.CrewLives + 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.CrewLives;
                __instance.Value = GameOptions.CrewLives;
                __instance.ValueText.Text = GameOptions.CrewLives.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.ExplosionRadius.TitleText.Text)
            {
                GameOptions.ExplosionRadius = Math.Min(GameOptions.ExplosionRadius + 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.ExplosionRadius;
                __instance.Value = GameOptions.ExplosionRadius;
                __instance.ValueText.Text = GameOptions.ExplosionRadius.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.BedrockDestroyTime.TitleText.Text)
            {
                GameOptions.BedrockDestroyTime = Math.Min(GameOptions.BedrockDestroyTime + 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.BedrockDestroyTime;
                __instance.Value = GameOptions.BedrockDestroyTime;
                __instance.ValueText.Text = GameOptions.BedrockDestroyTime.ToString();
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NumberOption.Decrease))]
        public static bool Prefix2(NumberOption __instance)
        {
            if (__instance.TitleText.Text == GameOptionsMenuPatch.TransformCooldown.TitleText.Text)
            {
                GameOptions.TransformCooldown = Math.Min(GameOptions.TransformCooldown - 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.TransformCooldown;
                __instance.Value = GameOptions.TransformCooldown;
                __instance.ValueText.Text = GameOptions.TransformCooldown.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.ExplodeCooldown.TitleText.Text)
            {
                GameOptions.ExplodeCooldown = Math.Min(GameOptions.ExplodeCooldown - 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.ExplodeCooldown;
                __instance.Value = GameOptions.ExplodeCooldown;
                __instance.ValueText.Text = GameOptions.ExplodeCooldown.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.BedrockCooldown.TitleText.Text)
            {
                GameOptions.BedrockCooldown = Math.Min(GameOptions.BedrockCooldown - 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.BedrockCooldown;
                __instance.Value = GameOptions.BedrockCooldown;
                __instance.ValueText.Text = GameOptions.BedrockCooldown.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.ReviveCooldown.TitleText.Text)
            {
                GameOptions.ReviveCooldown = Math.Min(GameOptions.ReviveCooldown - 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.ReviveCooldown;
                __instance.Value = GameOptions.ReviveCooldown;
                __instance.ValueText.Text = GameOptions.ReviveCooldown.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.BreakCooldown.TitleText.Text)
            {
                GameOptions.BreakCooldown = Math.Min(GameOptions.BreakCooldown - 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.BreakCooldown;
                __instance.Value = GameOptions.BreakCooldown;
                __instance.ValueText.Text = GameOptions.BreakCooldown.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.WitherDeathTime.TitleText.Text)
            {
                GameOptions.WitherDeathTime = Math.Min(GameOptions.WitherDeathTime - 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.WitherDeathTime;
                __instance.Value = GameOptions.WitherDeathTime;
                __instance.ValueText.Text = GameOptions.WitherDeathTime.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.WitherSkullSpeedMultiplier.TitleText.Text)
            {
                GameOptions.WitherSkullSpeedMultiplier = Math.Min(GameOptions.WitherSkullSpeedMultiplier - 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.WitherSkullSpeedMultiplier;
                __instance.Value = GameOptions.WitherSkullSpeedMultiplier;
                __instance.ValueText.Text = GameOptions.WitherSkullSpeedMultiplier.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.CrewLives.TitleText.Text)
            {
                GameOptions.CrewLives = (int) Math.Min(GameOptions.CrewLives - 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.CrewLives;
                __instance.Value = GameOptions.CrewLives;
                __instance.ValueText.Text = GameOptions.CrewLives.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.ExplosionRadius.TitleText.Text)
            {
                GameOptions.ExplosionRadius = Math.Min(GameOptions.ExplosionRadius - 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.ExplosionRadius;
                __instance.Value = GameOptions.ExplosionRadius;
                __instance.ValueText.Text = GameOptions.ExplosionRadius.ToString();
                return false;
            }
            
            if (__instance.TitleText.Text == GameOptionsMenuPatch.BedrockDestroyTime.TitleText.Text)
            {
                GameOptions.BedrockDestroyTime = Math.Min(GameOptions.BedrockDestroyTime - 2.5f, 45);
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                __instance.Field_3 = GameOptions.BedrockDestroyTime;
                __instance.Value = GameOptions.BedrockDestroyTime;
                __instance.ValueText.Text = GameOptions.BedrockDestroyTime.ToString();
                return false;
            }

            return true;
        }
    }
}