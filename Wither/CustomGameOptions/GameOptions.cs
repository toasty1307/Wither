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
        public static float ExplosionRadius { get; set; } = 3f;
        public static float WitherDeathTime { get; set; } = 10f;
        public static float BedrockDestroyTime { get; set; } = 15f;
        public static float WitherSkullSpeedMultiplier { get; set; } = 0.01f;
        public static int CrewLives { get; set; } = 3;
        public static bool DestroyBedrock { get; set; } = false;
        public static bool CrewCanVent { get; set; } = true;
        
        public static void CreateOptions()
        {
            _ = new CustomNumberOption("transform_cooldown", "Transform Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            _ = new CustomNumberOption("break_cooldown", "Break Cooldown", 25f, 0.5f, 5f, 45f, "0.0s");
            _ = new CustomToggleOption("crew_can_vent", "Crew Can Vent", true);
            _ = new CustomToggleOption("destroy_bedrock", "Destroy Bedrock", true);
            _ = new CustomStringOption("s_option", "aBc", 0, new []{"one", "two", "three"});
        }
    }
}