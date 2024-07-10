using BepInEx.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace BSCP
{
    internal class BSCPConfig
    {
        public readonly ConfigEntry<float> movementMultiplier;
        public readonly ConfigEntry<float> sprintMultiplier;
        public readonly ConfigEntry<bool> enableTemporaryFreeze;
        public readonly ConfigEntry<float> freezeTime;

        public BSCPConfig(ConfigFile cfg) 
        {
            cfg.SaveOnConfigSet = false;

            movementMultiplier = cfg.Bind("General", "MovementMultiplier", 1f, "Base Movement Multiplier");
            sprintMultiplier = cfg.Bind("General", "SprintMultiplier", 0.75f, "Sprint Multiplier (when chasing a player). This multiplier is a multiplier to the actual sprint multiplier of the monster. So with a SprintMultiplier of 0.75, the final multiplier will be 4.5 * 0.75 = 3.375. Big slaps would run 3.375 times faster than they walk.");

            enableTemporaryFreeze = cfg.Bind("General.Freeze", "EnableTemporaryFreeze", false, "If enabled, Big Slaps will be frozen for {freezeTime} seconds when it spawns, giving players a chance to run for the diving bell. A sound will also be played on spawn to warn players");
            freezeTime = cfg.Bind("General.Freeze", "FreezeTime", 60f, "Duration of the freeze in seconds");

            cfg.Save();

            cfg.SaveOnConfigSet = true;
        }
    }
}
