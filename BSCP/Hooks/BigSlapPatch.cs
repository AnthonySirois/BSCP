using BepInEx.Logging;
using DefaultNamespace;
using System.Collections.Generic;
using UnityEngine;

namespace BSCP.Patches
{
    public class BigSlapPatch
    {
        private static Dictionary<Bot_BigSlap, float> containedMonsters = new Dictionary<Bot_BigSlap, float>();

        internal static void Init()
        {
            On.Bot_BigSlap.Start += BigSlap_Start_Nerf;

            if (BSCP.BoundConfig.enableTemporaryFreeze.Value)
            {
                On.Bot_BigSlap.Start += BigSlap_Start_WarnAndFreeze;
                On.Bot_BigSlap.ComputeState += BigSlap_ComputeState_WarnAndFreeze;
            }
        }

        private static void BigSlap_Start_Nerf(On.Bot_BigSlap.orig_Start orig, Bot_BigSlap self)
        {
            orig(self);

            GameObject root = self.transform.parent.gameObject;
            PlayerController botController = root.GetComponent<PlayerController>();

            if (botController != null)
            {
                botController.movementForce *= BSCP.BoundConfig.movementMultiplier.Value;
                botController.sprintMultiplier *= BSCP.BoundConfig.sprintMultiplier.Value;
            } else
            {
                BSCP.Logger.LogError("Failed to find BigSlap.PlayerController");
            }
        }

        private static void BigSlap_Start_WarnAndFreeze(On.Bot_BigSlap.orig_Start orig, Bot_BigSlap self)
        {
            orig(self);

            containedMonsters.TryAdd(self, Time.time + BSCP.BoundConfig.freezeTime.Value);

            PlayWarningSound(self);
        }

        private static void PlayWarningSound(Bot_BigSlap self)
        {
            Transform root = self.transform.parent;
            SFX_PlayOneShot[] sfxs = root.gameObject.GetComponentsInChildren<SFX_PlayOneShot>();
            foreach (var sfx in sfxs)
            {
                if (sfx.gameObject.name == "VoiceNotice")
                {
                    Vector3 playerPosition = Player.localPlayer.HeadPosition();
                    Vector3 direction = (root.position - playerPosition).normalized;
                    Vector3 soundPosition = playerPosition + 5 * direction;

                    PlaySFX(sfx, soundPosition);
                }
            }
        }

        private static void PlaySFX(SFX_PlayOneShot sfx, Vector3 position)
        {
            if (sfx.sfx)
            {
                SFX_Player.instance.PlaySFX(sfx.sfx, position, null, null, 1f, false, false, true, 0f, 0);
            }
            foreach (var childSfx in sfx.sfxs)
            {
                SFX_Player.instance.PlaySFX(childSfx, position, null, null, 1f, false, false, true, 0f, 0);
            }
        }

        private static void BigSlap_ComputeState_WarnAndFreeze(On.Bot_BigSlap.orig_ComputeState orig, Bot_BigSlap self)
        {
            if (containedMonsters.TryGetValue(self, out float containmentEndTime))
            {
                if (containmentEndTime < Time.time)
                {
                    containedMonsters.Remove(self);

                    orig(self);
                }
            }
            else
            {
                orig(self);
            }
        }
    }
}
