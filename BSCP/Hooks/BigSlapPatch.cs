using System.Collections.Generic;
using UnityEngine;

namespace BSCP.Patches
{
    public class BigSlapPatch
    {
        private const float CONTAINMENT_TIME = 10f;

        private static Dictionary<Bot_BigSlap, float> containedMonsters = new Dictionary<Bot_BigSlap, float>();

        internal static void Init()
        {
            /*
             *  Subscribe with 'On.Namespace.Type.Method += CustomMethod;' for each method you're patching.
             *  Or if you are writing an ILHook, use 'IL.' instead of 'On.'
             *  Note that not all types are in a namespace, especially in Unity games.
             */
            On.Bot_BigSlap.Start += BigSlap_Start;
            On.Bot_BigSlap.ComputeState += BigSlap_ComputeState;
        }

        private static void BigSlap_Start(On.Bot_BigSlap.orig_Start orig, Bot_BigSlap self)
        {
            orig(self);

            // MonoBehaviour.Destroy(self.gameObject.gameObject.gameObject.gameObject);
            

            // containedMonsters.TryAdd(self, Time.time + CONTAINMENT_TIME);
        }

        private static void BigSlap_ComputeState(On.Bot_BigSlap.orig_ComputeState orig, Bot_BigSlap self)
        {
            // MonoBehaviour.Destroy(self.gameObject.gameObject.gameObject);

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
