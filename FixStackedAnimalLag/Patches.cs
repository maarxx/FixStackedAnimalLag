using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace FixStackedAnimalLag
{
    [StaticConstructorOnStartup]
    class Main
    {
        static Main()
        {
            Log.Message("Hello from Harmony in scope: com.github.harmony.rimworld.maarx.fixstackedanimallag");
            var harmony = HarmonyInstance.Create("com.github.harmony.rimworld.maarx.fixstackedanimallag");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    //public static Vector3 PawnCollisionPosOffsetFor(Pawn pawn)
    [HarmonyPatch(typeof(PawnCollisionTweenerUtility))]
    [HarmonyPatch("PawnCollisionPosOffsetFor")]
    class PawnCollisionTweenerUtility_PawnCollisionPosOffsetFor
    {
        static bool Prefix(ref Pawn pawn, ref Vector3 __result)
        {
            if (pawn != null && pawn.AnimalOrWildMan())
            {
                __result = Vector3.zero;
                return false;
            }
            return true;
            //Log.Message("Hello from Harmony PawnCollisionTweenerUtility_PawnCollisionPosOffsetFor Prefix");
        }
    }
}
