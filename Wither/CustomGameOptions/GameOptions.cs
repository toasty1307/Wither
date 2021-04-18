using Reactor;
using UnityEngine;
using Wither.Components.Option;

namespace Wither.CustomGameOptions
{
    public static class GameOptions
    {
        public static float TransformCooldown { get; set; } = 5f;
        public static float ExplodeCooldown { get; set; } = 5f;
        public static float BreakCooldown { get; set; } = 5f;
        public static float BedrockCooldown { get; set; } = 5f;
        public static float SkullCooldown { get; set; } = 5f;
        public static float ReviveCooldown { get; set; } = 5f;
        public static float MilkCooldown { get; set; } = 5f;
        public static float FixCooldown { get; set; } = 5f;
        public static float PotionCooldown { get; set; } = 5f;
        public static float PotionEffectCooldown { get; set; } = 5f;
        public static float ExplosionRadius { get; set; } = 1f;
        public static float WitherDeathTime { get; set; } = 10f;
        public static float BedrockDestroyTime { get; set; } = 15f;
        public static float FixRange { get; set; } = 1f;
        public static float WitherSkullSpeedMultiplier { get; set; } = 0.01f;
        public static int CrewLives { get; set; } = 3;
        public static bool DestroyBedrock { get; set; }
        public static bool CrewCanVent { get; set; } = true;
        public static int MilkUses { get; set; } = 3;
        public static int PotionUses { get; set; } = 3;

        public static void CreateOptions()
        {
            var transformCooldown = new CustomNumberOption("transform_cooldown", "Transform Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            transformCooldown.OnValueChanged += (_, args) => TransformCooldown = ((float) args.Value);
            
            var explodeCooldown   = new CustomNumberOption(  "explode_cooldown",   "Explode Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            explodeCooldown.OnValueChanged += (_, args) => ExplodeCooldown = ((float) args.Value);
            
            var breakCooldown     = new CustomNumberOption(    "break_cooldown",     "Break Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            breakCooldown.OnValueChanged += (_, args) => BreakCooldown = ((float) args.Value);
            
            var skullCooldown     = new CustomNumberOption(    "skull_cooldown",     "Skull Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            skullCooldown.OnValueChanged += (_, args) => SkullCooldown = ((float) args.Value);
            
            var milkCooldown      = new CustomNumberOption(     "milk_cooldown",      "Milk Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            milkCooldown.OnValueChanged += (_, args) => MilkCooldown = ((float) args.Value);
            
            var fixCooldown      = new CustomNumberOption(     "fix_cooldown",      "Fix Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            fixCooldown.OnValueChanged += (_, args) => FixCooldown = ((float) args.Value);
            
            var reviveCooldown    = new CustomNumberOption(   "revive_cooldown",    "Revive Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            reviveCooldown.OnValueChanged += (_, args) => ReviveCooldown = ((float) args.Value);
            
            var potionCooldown    = new CustomNumberOption(   "potion_cooldown",    "Potion Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            potionCooldown.OnValueChanged += (_, args) => PotionCooldown = ((float) args.Value);
            
            var potionEffectCooldown    = new CustomNumberOption(   "potion_effect_cooldown",    "Potion Effect Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            potionEffectCooldown.OnValueChanged += (_, args) => PotionEffectCooldown = ((float) args.Value);
            
            var bedrockCooldown   = new CustomNumberOption(  "bedrock_cooldown",   "Bedrock Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            bedrockCooldown.OnValueChanged += (_, args) => BedrockCooldown = ((float) args.Value);
            
            var witherDeathTime   = new CustomNumberOption(  "wither_death_time",   "Wither Death Time", 10f, 0.5f, 5f, 30f, "0.0s");
            witherDeathTime.OnValueChanged += (_, args) => WitherDeathTime = ((float) args.Value);
            
            var explosionRadius   = new CustomNumberOption(  "explosion_radius",   "Explosion Radius", 1f, 1f, 0.25f, 5f, "0.00x");
            explosionRadius.OnValueChanged += (_, args) => ExplosionRadius = ((float) args.Value);
            
            var bedrockDestroyTime   = new CustomNumberOption("bedrock_destroy_time",   "Bedrock Destroy Time", 250f, 10f, 100f, 250f, "0.00s");
            bedrockDestroyTime.OnValueChanged += (_, args) => BedrockDestroyTime = ((float) args.Value);
            
            var witherSkullSpeedMultiplier   = new CustomNumberOption("wither_skull_speed_multiplier", "Wither Skull Speed Multiplier", 0.25f, 0.25f, 0.25f, 1f, "0.0x");
            witherSkullSpeedMultiplier.OnValueChanged += (_, args) => WitherSkullSpeedMultiplier = ((float) args.Value);
            
            var fixRange   = new CustomNumberOption("fix_range", "Fix Range", 1f, 0.25f, 0.25f, 2f, "0.0x");
            fixRange.OnValueChanged += (_, args) => FixRange = ((float) args.Value);
            
            var crewLives   = new CustomNumberOption("crew_lives", "Crew Lives", 3, 1, 1, 10f, "0");
            crewLives.OnValueChanged += (_, args) => CrewLives = (int)((float) args.Value);
            
            var crewCanVent = new CustomToggleOption("crew_can_vent", "Crew Can Vent", true);
            crewCanVent.OnValueChanged += (_, args) => CrewCanVent = ((bool) args.Value);
            
            var destroyBedrock = new CustomToggleOption("destroy_bedrock", "Destroy Bedrock", true, Color.blue);
            destroyBedrock.OnValueChanged += (_, args) => DestroyBedrock = ((bool) args.Value);
            
            var milkUses   = new CustomNumberOption("milk_uses", "Milk Uses", 3, 1, 1, 10f, "0");
            milkUses.OnValueChanged += (_, args) => MilkUses = (int)((float) args.Value);

            var dropTest = new CustomDropdownOption("drop_test", "Drop Test", new[] {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"}, 0);
            
            var potionUses   = new CustomNumberOption("potion_uses", "Potion Uses", 3, 1, 1, 10f, "0");
            potionUses.OnValueChanged += (_, args) => PotionUses = (int)(float) args.Value;

            CustomOption.UpdateAll();
        }
    }
}