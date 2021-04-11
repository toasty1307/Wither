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
        public static float ExplosionRadius { get; set; } = 1f;
        public static float WitherDeathTime { get; set; } = 10f;
        public static float BedrockDestroyTime { get; set; } = 15f;
        public static float WitherSkullSpeedMultiplier { get; set; } = 0.01f;
        public static int CrewLives { get; set; } = 3;
        public static bool DestroyBedrock { get; set; }
        public static bool CrewCanVent { get; set; } = true;
        
        public static void CreateOptions()
        {
            var transformCooldown = new CustomNumberOption("transform_cooldown", "Transform Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            transformCooldown.OnValueChanged += (_, args) => Handlers.TransformCooldown((float) args.Value);
            var explodeCooldown   = new CustomNumberOption(  "explode_cooldown",   "Explode Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            explodeCooldown.OnValueChanged += (_, args) => Handlers.ExplodeCooldown((float) args.Value);
            var breakCooldown     = new CustomNumberOption(    "break_cooldown",     "Break Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            breakCooldown.OnValueChanged += (_, args) => Handlers.BreakCooldown((float) args.Value);
            var skullCooldown     = new CustomNumberOption(    "skull_cooldown",     "Skull Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            skullCooldown.OnValueChanged += (_, args) => Handlers.SkullCooldown((float) args.Value);
            var milkCooldown      = new CustomNumberOption(     "milk_cooldown",      "Milk Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            milkCooldown.OnValueChanged += (_, args) => Handlers.MilkCooldown((float) args.Value);
            var reviveCooldown    = new CustomNumberOption(   "revive_cooldown",    "Revive Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            reviveCooldown.OnValueChanged += (_, args) => Handlers.ReviveCooldown((float) args.Value);
            var bedrockCooldown   = new CustomNumberOption(  "bedrock_cooldown",   "Bedrock Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            bedrockCooldown.OnValueChanged += (_, args) => Handlers.BedrockCooldown((float) args.Value);
            var witherDeathTime   = new CustomNumberOption(  "wither_death_time",   "Wither Death Time", 10f, 0.5f, 5f, 30f, "0.0s");
            witherDeathTime.OnValueChanged += (_, args) => Handlers.WitherDeathTime((float) args.Value);
            var explosionRadius   = new CustomNumberOption(  "explosion_radius",   "Explosion Radius", 1f, 0.25f, 0.25f, 5f, "0.00x");
            explosionRadius.OnValueChanged += (_, args) => Handlers.ExplosionRadius((float) args.Value);
            var bedrockDestroyTime   = new CustomNumberOption("bedrock_destroy_time",   "Bedrock Destroy Time", 250f, 10f, 100f, 250f, "0.00s");
            bedrockDestroyTime.OnValueChanged += (_, args) => Handlers.BedrockDestroyTime((float) args.Value);
            var witherSkullSpeedMultiplier   = new CustomNumberOption("wither_skull_speed_multiplier", "Wither Skull Speed Multiplier", 0.25f, 0.25f, 0.25f, 1f, "0.0x");
            witherSkullSpeedMultiplier.OnValueChanged += (_, args) => Handlers.WitherSkullSpeedMultiplier((float) args.Value);
            var crewLives   = new CustomNumberOption("crew_lives", "Crew Lives", 3, 1, 1, 10f, "0");
            crewLives.OnValueChanged += (_, args) => Handlers.CrewLives((float) args.Value);
            var crewCanVent = new CustomToggleOption("crew_can_vent", "Crew Can Vent", true);
            crewCanVent.OnValueChanged += (_, args) => Handlers.CrewCanVent((bool) args.Value);
            var destroyBedrock = new CustomToggleOption("destroy_bedrock", "Destroy Bedrock", true, Color.blue);
            destroyBedrock.OnValueChanged += (_, args) => Handlers.DestroyBedrock((bool) args.Value);

        }

        public static class Handlers
        {
            public static void TransformCooldown(float value)
            {
                GameOptions.TransformCooldown = value;
            }
            
            public static void ExplodeCooldown(float value)
            {
                GameOptions.ExplodeCooldown = value;
            }
            
            public static void MilkCooldown(float value)
            {
                GameOptions.MilkCooldown = value;
            }
            
            public static void BreakCooldown(float value)
            {
                GameOptions.BreakCooldown = value;
            }
            
            public static void BedrockCooldown(float value)
            {
                GameOptions.BedrockCooldown = value;
            }
            
            public static void SkullCooldown(float value)
            {
                GameOptions.SkullCooldown = value;
            }
            
            public static void ReviveCooldown(float value)
            {
                GameOptions.ReviveCooldown = value;
            }
            
            public static void ExplosionRadius(float value)
            {
                GameOptions.ExplosionRadius = value;
            }
            
            public static void WitherDeathTime(float value)
            {
                GameOptions.WitherDeathTime = value;
            }
            
            public static void BedrockDestroyTime(float value)
            {
                GameOptions.BedrockDestroyTime = value;
            }
            
            public static void WitherSkullSpeedMultiplier(float value)
            {
                GameOptions.WitherSkullSpeedMultiplier = value;
            }
            
            public static void CrewLives(float value)
            {
                GameOptions.CrewLives = (int) value;
            }
            
            public static void DestroyBedrock(bool value)
            {
                GameOptions.DestroyBedrock = value;
            }
            
            public static void CrewCanVent(bool value)
            {
                GameOptions.CrewCanVent = value;
            }
        }
    }
}