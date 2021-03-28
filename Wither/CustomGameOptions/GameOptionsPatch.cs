using HarmonyLib;

namespace Wither.CustomGameOptions
{
    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
    public static class GameOptionsPatch
    {
        public static void Postfix()
        {
        }
    }

    public static class OnValueChanged
    {
        public static void CrewCanVent(bool x)
        {
            GameOptions.CrewCanVent = x;
        }
        public static void DestroyBedrock(bool x)
        {
            GameOptions.DestroyBedrock = x;
        }
        
        public static void BedrockCooldown(float x)
        {
            GameOptions.BedrockCooldown = x;
        }
    }
}