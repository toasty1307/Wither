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
        public static float ExplosionRadius { get; set; } = 3f;
        public static float WitherDeathTime { get; set; } = 10f;
        public static float BedrockDestroyTime { get; set; } = 15f;
        public static float WitherSkullSpeedMultiplier { get; set; } = 0.01f;
        public static int CrewLives { get; set; } = 3;
        
        public static bool DestroyBedrock { get; set; } = false;
    }
}