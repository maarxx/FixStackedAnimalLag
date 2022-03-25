using HarmonyLib;
using UnityEngine;
using Verse;

namespace FixStackedAnimalLag;

[HarmonyPatch(typeof(PawnCollisionTweenerUtility))]
[HarmonyPatch("PawnCollisionPosOffsetFor")]
internal class PawnCollisionTweenerUtility_PawnCollisionPosOffsetFor
{
    private static bool Prefix(ref Pawn pawn, ref Vector3 __result)
    {
        if (pawn == null || !pawn.AnimalOrWildMan())
        {
            return true;
        }

        __result = Vector3.zero;
        return false;
    }
}