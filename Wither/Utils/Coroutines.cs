using System.Collections.Generic;
using Wither.CustomGameOptions;

namespace Wither.Utils
{
    public static class Withering
    {
        public static Dictionary<PlayerControl, float> currentlyWithered = new Dictionary<PlayerControl, float>();

        public static PlayerControl wither;
        
        public static void Wither(PlayerControl source, PlayerControl target)
        {
            currentlyWithered.Add(target, GameOptions.WitherDeathTime);
        }
    }
}